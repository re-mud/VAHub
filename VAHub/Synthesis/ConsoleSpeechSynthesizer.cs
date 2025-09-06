using VAHub.Logging;

namespace VAHub.Synthesis;

public class ConsoleSpeechSynthesizer : ISpeechSynthesizer
{
    public void Speak(string text)
    {
        Logger.Info(text);
    }
}
