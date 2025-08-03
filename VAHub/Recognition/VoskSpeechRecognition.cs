using Vosk;
using System.Text.Json;
using VAHub.Logging;

namespace VAHub.Recognition;

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
        {
            Logger.Error("Некорректный размер буфера");
            throw new ArgumentException("Invalid buffer length");
        }

        _dataProcessed += length;

        return _recognizer.AcceptWaveform(buffer, length);
    }

    public void Reset()
    {
        if (_dataProcessed > _options.ThresholdSec * _options.SampleRate)
        {
            Logger.Debug("Сброс Vosk распознавателя");
            _recognizer.Dispose();
            _recognizer = new(_model, _options.SampleRate);
            _dataProcessed = 0;
        }
        else
        {
            _recognizer.Reset();
        }
    }

    public string Result()
    {
        return JsonDocument.Parse(_recognizer.Result())
            .RootElement.GetProperty("text").GetString() ?? string.Empty;
    }
}