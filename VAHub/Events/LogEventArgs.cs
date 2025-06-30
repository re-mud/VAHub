using VAHub.Enums;

namespace VAHub.Events;

public class LogEventArgs(string message, string text, LogLevel level, DateTime time) : EventArgs
{
    /// <summary>
    /// formatted log
    /// </summary>
    public readonly string Message = message;
    public readonly string Text = text;
    public readonly LogLevel Level = level;
    public readonly DateTime Time = time;
}