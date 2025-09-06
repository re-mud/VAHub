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
    private CommandContext? _context;

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

        if (_context == null && _isActivationPhraseEnabled)
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
            result = _commandManager.Handle(text, _context);
        }
        catch (Exception e)
        {
            Logger.Error($"Произошла неожиданная ошибка '{e.Message}'");
            return;
        }

        _context = null;

        HandleResult(result);
    }

    private void HandleResult(CommandManagerResult result)
    {
        if (_isActivationPhraseEnabled && _options.isExtendActivationAfterCommand && result.Status != Status.NotFound)
            _activationEnd = DateTime.UtcNow + _activationTimeout;

        HandleMessage(result);

        if (result.Status == Status.Success)
        {
            HandleResponse(result);
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

    private void HandleResponse(CommandManagerResult result)
    {
        if (result.CommandResponse == null) return;

        // Action
        switch (result.CommandResponse.Action)
        {
            case ActionType.None:
                break;

            case ActionType.Close:
                Exit();
                break;
        }

        // Speak
        if (!string.IsNullOrEmpty(result.CommandResponse.Speak))
        {
            _core.Speak(result.CommandResponse.Speak);
        }

        // Context
        if (!string.IsNullOrEmpty(result.CommandResponse.Context) && 
            result.CommandType.HasValue && 
            result.CommandPath != null)
        {
            Logger.Debug($"Установлен контекст '{result.CommandResponse.Context}'");
            _context = new(result.CommandType.Value, result.CommandResponse.Context, result.CommandPath);
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