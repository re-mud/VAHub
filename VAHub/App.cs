using VAHub.Commands.DTO;
using VAHub.Commands;
using VAHub.Logging;
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

        CommandManagerResult result;
        try
        {
            result = _commandManager.Handle(text);
        }
        catch (Exception e)
        {
            Logger.Error($"Произошла неожиданная ошибка '{e.Message}'");
            return;
        }

        HandleResult(result);
    }

    private void HandleResult(CommandManagerResult result)
    {
        if (_isActivationPhraseEnabled && _options.isExtendActivationAfterCommand && result.Status != Status.NotFound)
            _activationEnd = DateTime.UtcNow + _activationTimeout;

        HandleMessage(result);

        if (result.Status == Status.Success && result.CommandResponse != null)
        {
            HandleResponseAction(result.CommandResponse);
            HandleResponseSpeak(result.CommandResponse);
        }
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

    private void HandleResponseSpeak(CommandResponse response)
    {
        if (!string.IsNullOrEmpty(response.Speak))
        {
            _core.Speak(response.Speak);
        }
    }

    private void HandleResponseAction(CommandResponse response)
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

    private void HandleMessage(CommandManagerResult result)
    {
        if (string.IsNullOrEmpty(result.Message)) return;

        if (result.Status == Status.Error)
        {
            Logger.Error(result.Message);
        }
        else
        {
            Logger.Debug(result.Message);
        }
    }
}