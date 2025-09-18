namespace VAHub.Logging;

public interface ILogger
{
    void SetLevelEnabled(LogLevel level, bool enabled);

    public void Help(string text);

    public void Info(string text);

    public void Debug(string text);

    public void Warn(string text);

    public void Error(string text);

    public void Fatal(string text);
}
