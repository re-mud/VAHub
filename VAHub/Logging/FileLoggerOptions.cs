namespace VAHub.Options;

public class FileLoggerOptions
{
    public string LogPath { get; set; } = "Logs\\app.log";
    public string MessageFormat { get; set; } = "[{0}] [{1}] {2}\n";
    public string DateFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";
}