namespace VAHub.Commands;

public class CommandContext
{
    public CommandType CommandType;

    public string CommandData;

    public string CommandPath;

    public CommandContext(CommandType commandType, string commandData, string commandPath)
    {
        CommandType = commandType;
        CommandData = commandData;
        CommandPath = commandPath;
    }
}