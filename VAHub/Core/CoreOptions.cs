namespace VAHub.Core;

public class CoreOptions
{
    public string MicrophoneKey { get; set; } = "NAudioMicrophone";
    public string RecognitionKey { get; set; } = "VoskSpeechRecognition";
    public string SynthesizerKey { get; set; } = "WindowsSpeechSynthesizer";
}
