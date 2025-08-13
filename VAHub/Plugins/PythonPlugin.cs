using System.Text.Json;
using Python.Runtime;

using VAHub.Enums;
using VAHub.Models;

namespace VAHub.Plugins;

public class PythonPlugin : BasePlugin
{
    private Dictionary<string, PyObject> _pyModules = new();

    public PythonPlugin(string path, Manifest manifest) : base(path, manifest)
    {

    }

    public override Response Execute(string command, string text)
    {
        if (string.IsNullOrEmpty(command))
            return new(Status.Error, "Пустая команда");

        string[] parts = command.Split(':');
        if (parts.Length != 2)
            return new(Status.Error, "Неверный формат команды");

        string moduleName = parts[0];
        string funcName = parts[1];
        if (!_pyModules.TryGetValue(moduleName, out PyObject? pyModule))
        {
            try
            {
                pyModule = LoadModule(PluginDir, moduleName);
            }
            catch (FileNotFoundException)
            {
                return new(Status.Error, $"Не найден указанный файл '{moduleName}'");
            }
            catch (Exception e)
            {
                return new(Status.Error, $"Не удалось загрузить модуль '{moduleName}'");
            }
        }

        try
        {
            string json;
            using (Py.GIL())
            {
                dynamic func = pyModule.GetAttr(funcName);
                json = func(text);
            }

            return JsonSerializer.Deserialize<Response>(json) ?? throw new JsonException();
        }
        catch (Exception e)
        {
            return new(Status.Error, e.Message);
        }
    }

    /// <exception cref="Exception"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    private PyObject LoadModule(string path, string moduleName)
    {
        string modulePath = Path.Combine(path, moduleName);
        if (!File.Exists(modulePath)) throw new FileNotFoundException(modulePath);

        PyObject module;
        using (Py.GIL())
        {
            dynamic sys = Py.Import("sys");
            if (sys.path.count(path).As<int>() == 0)
                sys.path.insert(0, path);

            module = Py.Import(moduleName.Substring(0, moduleName.Length - 3));
            _pyModules[moduleName] = module;
        }

        return module;
    }
}
