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
            Report report = _commandManager.Handle(text);

            if (report.Message != "")
                HandleMessage(report);
            if (report.Status != Status.Success || report.Response == null)
                return;
            if (report.Response.Action != ActionType.None)
                HandleAction(report.Response.Action);
            if (report.Response.Speak != "")
                _core.Speak(report.Response.Speak);
        }
        catch (Exception e)
        {
            Logger.Error($"Произошла неожиданная ошибка '{e.Message}'");
        }
    }

    private void HandleAction(ActionType action)
    {
        switch (action)
        {
            case ActionType.Close:
                Exit();
                break;
        }
    }

    private void HandleMessage(Report report)
    {
        if (report.Status == Status.Error)
        {
            Logger.Error(report.Message);
        }
        else
        {
            Logger.Debug(report.Message);
        }
    }
}