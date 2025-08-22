using VAHub.Models;

namespace VAHub.Commands.Handlers;

public abstract class BaseCommandHandler
{
    public abstract Response Execute(string executeData, string path, string commandText);
}
