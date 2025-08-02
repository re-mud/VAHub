using VAHub.Events;

namespace VAHub.Input;

public interface IMicrophone
{
    public event EventHandler<MicrophoneEventArgs> DataAvailable;
    public int DeviceNumber { get; set; }
    public void Start();
    public void Stop();
}