namespace VAHub.Recognition;

public class VoskSpeechRecognitionOptions
{
    public string ModelPath { set; get; } = string.Empty;
    public int SampleRate { set; get; } = 16000;
    public int ThresholdSec { set; get; } = 120;
}
