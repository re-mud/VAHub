using System.Runtime.InteropServices;
using System.Speech.Synthesis;

using VAHub.Logging;

namespace VAHub.Synthesis;

public class WindowsSpeechSynthesizer : ISpeechSynthesizer
{
    private readonly SpeechSynthesizer _speechSynthesizer;
    private readonly WindowsSpeechSynthesizerOptions _options;
    private readonly ILogger _logger;

    public WindowsSpeechSynthesizer(ILogger logger, WindowsSpeechSynthesizerOptions options)
    {
        _logger = logger;

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            _logger.Error($"Синтез речи с '{nameof(WindowsSpeechSynthesizer)}' доступно только в Windows");
            throw new PlatformNotSupportedException("Available on Windows only.");
        }

        _speechSynthesizer = new SpeechSynthesizer();
        _options = options;

        _speechSynthesizer.Rate = _options.Rate;
        _speechSynthesizer.Volume = _options.Volume;
        if (_options.Voice != null)
            _speechSynthesizer.SelectVoice(_options.Voice);
    }

    public void Speak(string text)
    {
        _speechSynthesizer.Speak(text);
    }
}
