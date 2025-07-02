using VAHub.Enums;
using VAHub.Interfaces;
using VAHub.Options;

namespace VAHub.Logging;

public class FileLogger : ILogger
{
    private readonly FileLoggerOptions _options;
    private readonly object _lockFile = new();

    /// <param name="logPath">
    /// Disable logging to file if equal to null.
    /// </param>
    /// <param name="messageFormat">
    /// {0} - date<br></br>
    /// {1} - log level<br></br>
    /// {2} - message
    /// </param>
    public FileLogger(FileLoggerOptions options)
    {
        _options = options;

        if (!string.IsNullOrWhiteSpace(_options.LogPath))
        {
            CreateFileLog(_options.LogPath);
        }
    }

    public string Log(string text, LogLevel level, DateTime time)
    {
        lock (_lockFile)
        {
            try
            {
                string message = string.Format(_options.MessageFormat,
                    time.ToString(_options.DateFormat),
                    level.ToString(),
                    text);

                if (!string.IsNullOrWhiteSpace(_options.LogPath))
                {
                    File.AppendAllText(_options.LogPath, message);
                }

                return message;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return ex.Message;
            }
        }
    }

    private void CreateFileLog(string logPath)
    {
        string? directoryName = Path.GetDirectoryName(logPath);
        if (directoryName != null)
        {
            Directory.CreateDirectory(directoryName);
        }
        File.WriteAllText(logPath, string.Empty);
    }
}
