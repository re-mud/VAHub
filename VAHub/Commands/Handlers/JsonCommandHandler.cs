using System.Text.Json;
using VAHub.Models;

namespace VAHub.Commands.Handlers;

public class JsonCommandHandler : BaseCommandHandler
{
    public override Report Execute(string executeData, string path, string commandText)
    {
        if (string.IsNullOrEmpty(executeData))
            return Report.Error("Пустая команда");

        try
        {
            Response response = JsonSerializer.Deserialize<Response>(executeData) ?? throw new JsonException();
            
            return Report.Success("", response, CommandType.Json);
        }
        catch
        {
            return Report.Error("Команда имеет неверный формат");
        }
    }
}
