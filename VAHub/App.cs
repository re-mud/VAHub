using VAHub.Commands;
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
    private CommandManager _commandManager;

    public App(VACore core, CommandManager commandManager)
    {
        _core = core;
        _commandManager = commandManager;
        _mre = new ManualResetEventSlim(false);

        _core.RecognitionCompleted += Core_RecognitionCompleted;
    }

    public void Run()
    {
        _core.Start();
        Logger.Debug("Приложение запущено");
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
            Response response = _commandManager.Handle(text);

            if (response.Status == Status.Error)
            {
                if (response.Message != "")
                    Logger.Error(response.Message);
            }
            else
            {
                if (response.Status == Status.Success)
                    HandleResponse(response);

                if (response.Message != "")
                    Logger.Debug(response.Message);
            }
        }
        catch (Exception e)
        {
            Logger.Error($"Произошла неожиданная ошибка '{e.Message}'");
        }
    }
}