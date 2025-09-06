using VAHub.Commands.DTO;

namespace VAHub.Commands.Handlers;

public abstract class BaseCommandHandler
{
    public abstract CommandResult Execute(string executeData, string path, string commandText);
}
