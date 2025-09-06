using VAHub.Enums;

namespace VAHub.Commands.DTO;

public class CommandResult
{
    public CommandResponse? CommandResponse { get; set; }

    public string? Message { get; set; }

    public Status Status { get; set; }

    public CommandResult(
        Status status,
        string? message = null,
        CommandResponse? commandResponse = null)
    {
        Status = status;
        Message = message;
        CommandResponse = commandResponse;
    }
}
