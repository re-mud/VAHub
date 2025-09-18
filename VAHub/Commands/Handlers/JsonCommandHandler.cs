using System.Text.Json;
using VAHub.Commands.DTO;
using VAHub.Commands.Enums;

namespace VAHub.Commands.Handlers;

public class JsonCommandHandler : BaseCommandHandler
{
    public override CommandResult Execute(string executeData, string path, string commandText)
    {
        if (string.IsNullOrEmpty(executeData))
            return new(Status.Error, "Пустая команда");

        try
        {
            CommandResponse result = JsonSerializer.Deserialize<CommandResponse>(executeData) ?? throw new JsonException();
            
            return new(Status.Success, commandResponse: result);
        }
        catch
        {
            return new(Status.Error, "Команда имеет неверный формат");
        }
    }
}
