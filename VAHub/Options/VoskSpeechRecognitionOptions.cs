namespace VAHub.Options;

public class VoskSpeechRecognitionOptions
{
    public string ModelPath { set; get; } = string.Empty;
    public int SampleRate { set; get; } = 16000;
    public int ThresholdDataProcessed { set; get; } = 16000 * 120; // SampleRate * second
}
