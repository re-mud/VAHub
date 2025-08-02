using VAHub.Enums;

namespace VAHub.Logging;

public interface ILogger
{
    string Log(string text, LogLevel level, DateTime time);
}
