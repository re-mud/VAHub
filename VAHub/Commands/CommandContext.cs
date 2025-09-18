namespace VAHub.Commands;

public class CommandContext(CommandType commandType, string commandData, string commandPath)
{
    public CommandType CommandType = commandType;

    public string CommandData = commandData;

    public string CommandPath = commandPath;
}