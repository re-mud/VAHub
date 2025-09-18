namespace VAHub.Logging;

public class FileLogger : ILogger
{
    private readonly FileLoggerOptions _options;
    private readonly object _lockFile = new();
    private readonly StreamWriter? _writer;
    private readonly List<LogLevel> _disabledLogs = [];

    public FileLogger(FileLoggerOptions options)
    {
        _options = options;

        if (_options.LogPath != string.Empty)
        {
            if (!Directory.Exists(_options.LogPath))
                Directory.CreateDirectory(_options.LogPath);

            string filePath = Path.Combine(
                _options.LogPath, DateTime.Now.ToString(_options.FileDateFormat) + ".log");
            FileStream fileStream = new(
                filePath, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, false);
            _writer = new StreamWriter(fileStream);
            _writer.AutoFlush = true;
        }
    }

    public void SetLevelEnabled(LogLevel level, bool enabled)
    {
        if (enabled)
            _disabledLogs.Remove(level);
        else
            _disabledLogs.Add(level);
    }

    public void Debug(string text) => Log(text, LogLevel.Debug);

    public void Error(string text) => Log(text, LogLevel.Error);

    public void Fatal(string text) => Log(text, LogLevel.Fatal);

    public void Help(string text) => Log(text, LogLevel.Help);

    public void Info(string text) => Log(text, LogLevel.Info);

    public void Warn(string text) => Log(text, LogLevel.Warn);

    private void Log(string text, LogLevel level)
    {
        if (_disabledLogs.Contains(level)) return;

        DateTime time = DateTime.Now;

        lock (_lockFile)
        {
            try
            {
                string message = string.Format(
                    _options.MessageFormat, time.ToString(_options.LogDateFormat), level.ToString(), text);

                Console.WriteLine(message);
                _writer?.WriteLine(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
