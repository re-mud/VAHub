using VAHub.Events;

namespace VAHub.Interfaces;

public interface IMicrophone
{
    public event EventHandler<MicrophoneEventArgs> DataAvailable;
    public int DeviceNumber { get; set; }
    public void Start();
    public void Stop();
}