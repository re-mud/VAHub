using VAHub.Commands;
using VAHub.Logging;
using VAHub.Models;
using VAHub.Enums;
using VAHub.Core;

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

    private void Core_RecognitionCompleted(string text)
    {
        try
        {
            Response response = _commandManager.Handle(text);

            if (response.Message != "")
                HandleMessage(response);
            if (response.Status != Status.Success)
                return;
            if (response.Action != ActionType.None)
                HandleAction(response);
            if (response.Speak != "")
                _core.Speak(response.Speak);
        }
        catch (Exception e)
        {
            Logger.Error($"Произошла неожиданная ошибка '{e.Message}'");
        }
    }

    private void HandleAction(Response response)
    {
        switch (response.Action)
        {
            case ActionType.None:
                break;

            case ActionType.Close:
                Exit();
                break;
        }
    }

    private void HandleMessage(Response response)
    {
        if (response.Status == Status.Error)
        {
            Logger.Error(response.Message);
        }
        else
        {
            Logger.Debug(response.Message);
        }
    }
}