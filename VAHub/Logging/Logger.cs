namespace VAHub.Logging;

public static class Logger
{
    public static event EventHandler<LogEventArgs>? LogEvent;
    private static ILogger? _logger;

    public static void SetLogger(ILogger? logger) => _logger = logger;

    public static void Info(string text) => Log(text, LogLevel.Info);

    public static void Debug(string text) => Log(text, LogLevel.Debug);

    public static void Warn(string text) => Log(text, LogLevel.Warn);

    public static void Error(string text) => Log(text, LogLevel.Error);

    public static void Fatal(string text) => Log(text, LogLevel.Fatal);

    private static void Log(string text, LogLevel level)
    {
        if (_logger == null) return;

        DateTime time = DateTime.Now;
        string message = _logger.Log(text, level, time);

        LogEvent?.Invoke(null, new(message, text, level, time));
    }
}