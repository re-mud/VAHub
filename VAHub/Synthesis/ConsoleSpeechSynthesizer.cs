using VAHub.Logging;

namespace VAHub.Synthesis;

public class ConsoleSpeechSynthesizer : ISpeechSynthesizer
{
    private readonly ILogger _logger;

    public ConsoleSpeechSynthesizer(ILogger logger)
    {
        _logger = logger;
    }

    public void Speak(string text)
    {
        _logger.Info(text);
    }
}
