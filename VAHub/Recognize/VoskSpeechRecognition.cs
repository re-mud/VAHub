using Vosk;

using VAHub.Interfaces;
using VAHub.Options;

namespace VAHub.Recognize;

public class VoskSpeechRecognition : ISpeechRecognition
{
    private VoskSpeechRecognitionOptions _options;
    private VoskRecognizer _recognizer;
    private Model _model;
    private int _dataProcessed = 0;

    public VoskSpeechRecognition(VoskSpeechRecognitionOptions options)
    {
        _options = options;

        _model = new(_options.ModelPath);
        _recognizer = new(_model, _options.SampleRate);
    }

    /// <exception cref="ArgumentException"></exception>
    public bool Accept(byte[] buffer, int length)
    {
        if (length > buffer.Length) 
            throw new ArgumentException("Invalid buffer length");

        _dataProcessed += length;

        if (_recognizer.AcceptWaveform(buffer, length))
        {
            if (_dataProcessed > _options.ThresholdDataProcessed)
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
        _recognizer = new(_model, _options.SampleRate);
    }

    public string Result()
    {
        string result = _recognizer.Result();

        return result.Substring(14, result.Length - 17);
    }
}