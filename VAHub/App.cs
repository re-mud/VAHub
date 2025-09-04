using VAHub.Commands;
using VAHub.Logging;
using VAHub.Models;
using VAHub.Enums;
using VAHub.Core;

namespace VAHub;

public class App
{
    private readonly VACore _core;
    private readonly ManualResetEventSlim _mre;
    private readonly CommandManager _commandManager;
    private readonly AppOptions _options;
    private readonly bool _isActivationPhraseEnabled;
    private readonly TimeSpan _activationTimeout;
    private DateTime _activationEnd;

    public App(VACore core, CommandManager commandManager, AppOptions options)
    {
        _core = core;
        _options = options;
        _commandManager = commandManager;
        _mre = new ManualResetEventSlim(false);
        _isActivationPhraseEnabled = !string.IsNullOrEmpty(_options.ActivationPhrase);
        _activationTimeout = TimeSpan.FromSeconds(_options.ActivationTimeoutSeconds);

        _core.RecognitionCompleted += Core_RecognitionCompleted;
    }

    public void Run()
    {
        _core.Start();
        Logger.Debug("Приложение запущено");

        if (string.IsNullOrEmpty(_options.ActivationPhrase))
        {
            Logger.Debug($"Фраза активации выключена");
        }
        else
        {
            Logger.Debug($"Фраза активации '{_options.ActivationPhrase}'");
            Logger.Debug($"Время активации '{_options.ActivationTimeoutSeconds}'");
            Logger.Debug($"Продление активации '{_options.isExtendActivationAfterCommand}'");
        }
        _mre.Wait();
    }

    public void Exit()
    {
        _mre.Set();
    }

    private void Core_RecognitionCompleted(string text)
    {
        if (string.IsNullOrEmpty(text)) return;

        if (_isActivationPhraseEnabled)
        {
            if (IsActivatedCommand(text, out string command))
            {
                _activationEnd = DateTime.UtcNow + _activationTimeout;
                if (command == string.Empty) return;
                text = command;
            }
            else if (_activationEnd < DateTime.UtcNow)
            {
                Logger.Debug($"Вне активации '{text}'");
                return;
            }
        }

        Report report;
        try
        {
            report = _commandManager.Handle(text);
        }
        catch (Exception e)
        {
            Logger.Error($"Произошла неожиданная ошибка '{e.Message}'");
            return;
        }

        HandleReport(report);
    }

    private void HandleReport(Report report)
    {
        if (_isActivationPhraseEnabled && _options.isExtendActivationAfterCommand && report.Status != Status.NotFound)
            _activationEnd = DateTime.UtcNow + _activationTimeout;

        if (!string.IsNullOrEmpty(report.Message))
            HandleMessage(report);

        if (report.Status != Status.Success || report.Response == null)
            return;

        if (report.Response.Action != ActionType.None)
            HandleAction(report.Response.Action);

        if (!string.IsNullOrEmpty(report.Response.Speak))
            _core.Speak(report.Response.Speak);
    }

    private bool IsActivatedCommand(string text, out string command)
    {
        command = string.Empty;

        if (text == _options.ActivationPhrase)
        {
            return true;
        }
        if (text.StartsWith(_options.ActivationPhrase))
        {
            command = text.Substring(_options.ActivationPhrase.Length).Trim();
            return true;
        }
        return false;
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