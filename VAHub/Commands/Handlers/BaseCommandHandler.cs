using VAHub.Models;

namespace VAHub.Commands.Handlers;

public abstract class BaseCommandHandler
{
    public abstract Report Execute(string executeData, string path, string commandText);
}
