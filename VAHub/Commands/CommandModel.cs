namespace VAHub.Commands;

public class CommandModel
{
    public readonly string ExecuteData;
    public readonly string RelativePath;
    public readonly string Type;

    public CommandModel(string executeData, string type, string relativePath)
    {
        RelativePath = relativePath;
        ExecuteData = executeData;
        Type = type;
    }
}
