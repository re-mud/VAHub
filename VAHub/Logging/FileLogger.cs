using VAHub.Enums;
using VAHub.Interfaces;

namespace VAHub.Logging;

public class FileLogger : ILogger
{
    private readonly string? _logPath;
    private readonly string _messageFormat;
    private readonly string _dateFormat;
    private readonly object _lockFile = new();

    /// <param name="logPath">
    /// disable logging to file if equal to null
    /// </param>
    /// <param name="messageFormat">
    /// {0} - date<br></br>
    /// {1} - log level<br></br>
    /// {2} - message
    /// </param>
    public FileLogger(string? logPath, string messageFormat, string dateFormat)
    {
        _logPath = logPath;
        _messageFormat = messageFormat;
        _dateFormat = dateFormat;

        if (string.IsNullOrWhiteSpace(logPath))
        {
            _logPath = null;
        }
        else
        {
            CreateFileLog(_logPath);
        }
    }

    public string Log(string text, LogLevel level, DateTime time)
    {
        lock (_lockFile)
        {
            try
            {
                string message = string.Format(_messageFormat,
                    time.ToString(_dateFormat),
                    level.ToString(),
                    text);

                if (_logPath != null)
                {
                    File.AppendAllText(_logPath, message);
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
