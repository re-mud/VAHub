using System.Runtime.InteropServices;
using System.Speech.Synthesis;

namespace VAHub.Synthesize;

public class WindowsSpeechSynthesizer : ISpeechSynthesizer
{
    private SpeechSynthesizer _speechSynthesizer;
    private WindowsSpeechSynthesizerOptions _options;

    public WindowsSpeechSynthesizer(WindowsSpeechSynthesizerOptions options)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new PlatformNotSupportedException("Available on Windows only.");

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
