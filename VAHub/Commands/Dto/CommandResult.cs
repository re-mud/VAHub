using VAHub.Enums;

namespace VAHub.Commands.DTO;

public class CommandResult(
    Status status,
    string? message = null,
    CommandResponse? commandResponse = null)
{
    public CommandResponse? CommandResponse { get; set; } = commandResponse;

    public string? Message { get; set; } = message;

    public Status Status { get; set; } = status;
}
