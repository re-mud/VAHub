using System.Text.Json;
using VAHub.Logging;
using Vosk;

namespace VAHub.Recognition;

public class VoskSpeechRecognition : ISpeechRecognition
{
    private readonly VoskSpeechRecognitionOptions _options;
    private readonly ILogger _logger;
    private readonly Model _model;
    private VoskRecognizer _recognizer;
    private int _dataProcessed = 0;

    public VoskSpeechRecognition(ILogger logger, VoskSpeechRecognitionOptions options)
    {
        _logger = logger;
        _options = options;

        if (!Directory.Exists(_options.ModelPath))
        {
            _logger.Error($"Модель не найдена по пути {_options.ModelPath}");
            throw new FileNotFoundException($"Model not found at {_options.ModelPath}");
        }

        _model = new(_options.ModelPath);
        _recognizer = new(_model, _options.SampleRate);
    }

    /// <exception cref="ArgumentException"></exception>
    public bool Accept(byte[] buffer, int length)
    {
        if (length > buffer.Length)
        {
            _logger.Error("Некорректный размер буфера");
            throw new ArgumentException("Invalid buffer length");
        }

        _dataProcessed += length;

        return _recognizer.AcceptWaveform(buffer, length);
    }

    public void Reset()
    {
        if (_dataProcessed > _options.ThresholdSec * _options.SampleRate)
        {
            _logger.Debug("Сброс Vosk распознавателя");
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