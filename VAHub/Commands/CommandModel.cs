namespace VAHub.Commands;

public class CommandModel(string executeData, CommandType type, string relativePath)
{
    public readonly string ExecuteData = executeData;

    public readonly string RelativePath = relativePath;

    public readonly CommandType Type = type;
}
