namespace VAHub.Synthesize;

public class WindowsSpeechSynthesizerOptions
{
    /// <summary>
    /// [-10, 10]
    /// </summary>
    private int _rate = 0;
    public int Rate
    {
        get => _rate;
        set
        {
            if (value < -10 || value > 10)
                throw new ArgumentOutOfRangeException(nameof(Rate), "Valid range: [-10, 10]");
            _rate = value;
        }
    }

    /// <summary>
    /// [0, 100]
    /// </summary>
    private int _volume = 100;
    public int Volume
    {
        get => _volume;
        set
        {
            if (value < 0 || value > 100)
                throw new ArgumentOutOfRangeException(nameof(Volume), "Valid range: [0, 100]");
            _volume = value;
        }
    }

    /// <summary>
    /// null - default voice
    /// </summary>
    public string? Voice { get; set; } = null;
}