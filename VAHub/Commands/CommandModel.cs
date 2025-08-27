namespace VAHub.Commands;

public class CommandModel
{
    public readonly string ExecuteData;
    public readonly string RelativePath;
    public readonly CommandType Type;

    public CommandModel(string executeData, CommandType type, string relativePath)
    {
        RelativePath = relativePath;
        ExecuteData = executeData;
        Type = type;
    }
}
