namespace VAHub;

public class AppOptions
{
    public string ActivationPhrase { set; get; } = "";
    public int ActivationTimeoutSeconds { set; get; } = 15;
    public bool IsExtendActivationAfterCommand { set; get; } = false;
}
