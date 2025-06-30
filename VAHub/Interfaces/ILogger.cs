using VAHub.Enums;

namespace VAHub.Interfaces;

public interface ILogger
{
    string Log(string text, LogLevel level, DateTime time);
}
