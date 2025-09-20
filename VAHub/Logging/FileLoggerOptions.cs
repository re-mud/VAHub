namespace VAHub.Logging;

public class FileLoggerOptions
{
    public string LogPath { get; set; } = "logs\\";
    public string MessageFormat { get; set; } = "[{0}] [{1}] {2}";
    public string LogDateFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";
    public string FileDateFormat { get; set; } = "yyyy-MM-dd_HH-mm-ss";
}