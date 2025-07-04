namespace VAHub.Options;

public class NAudioMicrophoneOptions
{
    public int SampleRate { set; get; } = 16000;
    public int Bits { set; get; } = 16;
    public int Channels { set; get; } = 1;
}