using VAHub.Enums;

namespace VAHub.Commands.DTO
{
    public class CommandManagerResult : CommandResult
    {
        public CommandType? CommandType { get; set; }

        public string? CommandPath { get; set; }

        public CommandManagerResult(
            Status status,
            CommandType? commandType = null,
            string? commandPath = null,
            string? message = null,
            CommandResponse? commandResponse = null) : base(status, message, commandResponse)
        {
            CommandType = commandType;
            CommandPath = commandPath;
        }
    }
}
