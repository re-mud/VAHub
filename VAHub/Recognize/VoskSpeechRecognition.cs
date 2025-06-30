using Vosk;

using VAHub.Interfaces;

namespace VAHub.Recognize;

public class VoskSpeechRecognition : ISpeechRecognition
{
    private VoskRecognizer _recognizer;
    private Func<VoskRecognizer> _createVoskRecognizer;
    private int _thresholdDataProcessed;
    private int _dataProcessed = 0;

    public VoskSpeechRecognition(Func<VoskRecognizer> createVoskRecognizer, int thresholdDataProcessed)
    {
        ArgumentNullException.ThrowIfNull(createVoskRecognizer, nameof(createVoskRecognizer));

        _createVoskRecognizer = createVoskRecognizer;
        _thresholdDataProcessed = thresholdDataProcessed;
        _recognizer = _createVoskRecognizer();
    }

    /// <exception cref="ArgumentException"></exception>
    public bool Accept(byte[] buffer, int length)
    {
        if (length <= buffer.Length) throw new ArgumentException("Invalid buffer length");

        _dataProcessed += length;

        if (_recognizer.AcceptWaveform(buffer, length))
        {
            if (_dataProcessed > _thresholdDataProcessed)
            {
                _dataProcessed = 0;
                Reset();
            }

            return true;
        }
        return false;
    }

    public void Reset()
    {
        _recognizer.Dispose();
        _recognizer = _createVoskRecognizer();
    }

    public string Result()
    {
        string result = _recognizer.Result();

        return result.Substring(14, result.Length - 17);
    }
}