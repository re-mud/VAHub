namespace VAHub.Recognition;

public interface ISpeechRecognition
{
    public bool Accept(byte[] buffer, int length);
    public string Result();
    public void Reset();
}