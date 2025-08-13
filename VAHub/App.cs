using VAHub.Core;
using VAHub.Enums;
using VAHub.Logging;
using VAHub.Models;
using VAHub.Plugins;

namespace VAHub;

public class App
{
    private VACore _core;
    private ManualResetEventSlim _mre;
    private PluginManager _pluginManager;

    public App(VACore core, PluginManager pluginManager)
    {
        _core = core;
        _pluginManager = pluginManager;
        _mre = new ManualResetEventSlim(false);

        _core.RecognitionCompleted += Core_RecognitionCompleted;
    }

    public void Run()
    {
        _core.Start();
        Logger.Info("Приложение запущено");
        _mre.Wait();
    }

    public void Exit()
    {
        _mre.Set();
    }

    /// <exception cref="ArgumentNullException"></exception>
    public void HandleResponse(Response response)
    {
        ArgumentNullException.ThrowIfNull(response);

        switch (response.Action)
        {
            case ActionType.None:
                {
                    break;
                }

            case ActionType.Speak:
                {
                    if (response.Data != "")
                        _core.Speak(response.Data);
                    break;
                }

            case ActionType.Close:
                {
                    Exit();
                    break;
                }
        }
    }

    private void Core_RecognitionCompleted(string text)
    {
        try
        {
            Response response = _pluginManager.Handle(text);

            HandleResponse(response);

            Logger.Debug($"Статус: {response.Status}, сообщение: {response.Message}");
        }
        catch (Exception e)
        {
            Logger.Error($"Произошла неожиданная ошибка '{e.Message}'");
        }
    }
}