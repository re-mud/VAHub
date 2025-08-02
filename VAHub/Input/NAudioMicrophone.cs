using NAudio.Wave;

namespace VAHub.Input;

public class NAudioMicrophone : IMicrophone
{
    public event EventHandler<MicrophoneEventArgs>? DataAvailable;

    private NAudioMicrophoneOptions _options;
    private WaveInEvent _waveInEvent;

    public int DeviceNumber
    {
        get => _waveInEvent.DeviceNumber;
        set => _waveInEvent.DeviceNumber = value;
    }

    public NAudioMicrophone(NAudioMicrophoneOptions options)
    {
        _options = options;
        _waveInEvent = new();

        _waveInEvent.WaveFormat = new WaveFormat(_options.SampleRate, _options.Bits, _options.Channels);
        _waveInEvent.DataAvailable += WaveInEvent_DataAvailable;
    }

    public void Start()
    {
        _waveInEvent.StartRecording();
    }

    public void Stop()
    {
        _waveInEvent.StopRecording();
    }

    private void WaveInEvent_DataAvailable(object? sender, WaveInEventArgs e)
    {
        if (e.Buffer.Length != e.BytesRecorded) return;

        DataAvailable?.Invoke(this, new MicrophoneEventArgs(
            e.Buffer,
            e.BytesRecorded
        ));
    }
}