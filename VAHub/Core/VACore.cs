using VAHub.Input;
using VAHub.Recognition;
using VAHub.Synthesis;

namespace VAHub.Core;

public class VACore
{
    public event Action<string>? RecognitionCompleted;

    private IMicrophone _microphone;
    private ISpeechRecognition _recognition;
    private ISpeechSynthesizer _synthesizer;

    public VACore(IMicrophone microphone, ISpeechRecognition recognition, ISpeechSynthesizer synthesizer)
    {
        ArgumentNullException.ThrowIfNull(microphone, nameof(microphone));
        ArgumentNullException.ThrowIfNull(recognition, nameof(recognition));
        ArgumentNullException.ThrowIfNull(synthesizer, nameof(synthesizer));

        SetMicrophone(microphone);
        SetRecognition(recognition);
        SetSynthesizer(synthesizer);
    }

    public void SetMicrophone(IMicrophone microphone)
    {
        if (microphone == null) 
            throw new ArgumentNullException(nameof(microphone));
        if (_microphone != null)
            _microphone.DataAvailable -= Microphone_DataAvailable;

        _microphone = microphone;
        _microphone.DataAvailable += Microphone_DataAvailable;
    }

    public void SetRecognition(ISpeechRecognition recognition)
    {
        _recognition = recognition ?? throw new ArgumentNullException(nameof(recognition));
        _recognition.Reset();
    }

    public void SetSynthesizer(ISpeechSynthesizer synthesizer)
    {
        _synthesizer = synthesizer ?? throw new ArgumentNullException(nameof(synthesizer));
    }

    public void Speak(string text)
    {
        _synthesizer.Speak(text);
    }

    public void Start()
    {
        _microphone.Start();
    }

    public void Stop()
    {
        _microphone.Stop();
        _recognition.Reset();
    }

    private void Microphone_DataAvailable(object? sender, MicrophoneEventArgs e)
    {
        if (_recognition.Accept(e.Buffer, e.Length))
        {
            string result = _recognition.Result();

            if (result != string.Empty)
                RecognitionCompleted?.Invoke(result);

            _recognition.Reset();
        }
    }
}
