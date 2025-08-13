namespace VAHub.Logging;

public class FileLogger : ILogger
{
    private readonly FileLoggerOptions _options;
    private readonly object _lockFile = new();
    private readonly StreamWriter? _writer;

    public FileLogger(FileLoggerOptions options)
    {
        _options = options;

        if (!string.IsNullOrWhiteSpace(_options.LogPath))
        {
            FileStream fileStream = new FileStream(_options.LogPath, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, false);
            _writer = new StreamWriter(fileStream)
            {
                AutoFlush = true
            };
        }
    }

    public string Log(string text, LogLevel level, DateTime time)
    {
        lock (_lockFile)
        {
            try
            {
                string message = string.Format(_options.MessageFormat, time.ToString(_options.DateFormat), level.ToString(), text);

                Console.WriteLine(message);
                _writer?.WriteLine(message);

                return message;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return ex.Message;
            }
        }
    }
}
