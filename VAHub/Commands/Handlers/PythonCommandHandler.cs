using System.Text.Json;
using Python.Runtime;
using VAHub.Models;
using VAHub.Enums;
using VAHub.Logging;

namespace VAHub.Commands.Handlers;

public class PythonCommandHandler : BaseCommandHandler
{
    private Dictionary<string, PyObject> _pyModules = new();

    public PythonCommandHandler(PythonCommandHandlerOptions options)
    {
        Runtime.PythonDLL = options.PythonDLLPath;
        PythonEngine.Initialize();
        PythonEngine.BeginAllowThreads();
    }

    public override Report Execute(string executeData, string path, string commandText)
    {
        if (string.IsNullOrEmpty(executeData))
            return Report.Error("Пустая команда");

        string[] parts = executeData.Split(':');
        if (parts.Length != 2)
            return Report.Error("Неверный формат команды");

        string moduleName = parts[0];
        string funcName = parts[1];

        if (!_pyModules.TryGetValue(moduleName, out PyObject? pyModule))
        {
            try
            {
                LoadModule(path, moduleName);
                pyModule = _pyModules[moduleName];
            }
            catch (FileNotFoundException)
            {
                return Report.NotFound($"Не найден указанный файл '{Path.Combine(path, moduleName)}'");
            }
            catch (Exception)
            {
                return Report.Error($"Не удалось загрузить модуль '{Path.Combine(path, moduleName)}'");
            }
        }

        try
        {
            string json;
            using (Py.GIL())
            {
                dynamic func = pyModule.GetAttr(funcName);
                json = func(commandText);
            }

            Response response = JsonSerializer.Deserialize<Response>(json) ?? throw new JsonException();
            return Report.Success("",  response, CommandType.Python);
        }
        catch (Exception e)
        {
            return Report.Success(e.Message);
        }
    }

    /// <exception cref="Exception"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    private void LoadModule(string path, string moduleName)
    {
        string modulePath = Path.Combine(path, moduleName);
        if (!File.Exists(modulePath)) throw new FileNotFoundException(modulePath);

        using (Py.GIL())
        {
            dynamic sys = Py.Import("sys");
            if (sys.path.count(path).As<int>() == 0)
                sys.path.insert(0, path);

            PyObject module = Py.Import(Path.GetFileNameWithoutExtension(moduleName));
            _pyModules[moduleName] = module;
        }
    }
}
