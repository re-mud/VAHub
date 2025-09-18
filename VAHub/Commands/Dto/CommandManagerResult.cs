using VAHub.Enums;

namespace VAHub.Commands.DTO;

public class CommandManagerResult(
    Status status,
    CommandType? commandType = null,
    string? commandPath = null,
    string? message = null,
    CommandResponse? commandResponse = null) : CommandResult(status, message, commandResponse)
{
    public CommandType? CommandType { get; set; } = commandType;

    public string? CommandPath { get; set; } = commandPath;
}
