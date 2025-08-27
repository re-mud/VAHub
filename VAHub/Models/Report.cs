using VAHub.Commands;
using VAHub.Enums;

namespace VAHub.Models;

public class Report
{
    private Report(
        Status status,
        string message = "",
        Response? response = null,
        CommandType commandType = CommandType.None)
    {
        Status = status;
        Message = message;
        Response = response;
        CommandType = commandType;
    }

    public readonly Status Status;

    public readonly string Message;

    public readonly Response? Response;

    public readonly CommandType CommandType;

    public static Report Success(
        string message,
        Response response,
        CommandType commandType)
    {
        return new(Status.Success, message, response, commandType);
    }

    public static Report Success(string message = "")
    {
        return new(Status.Success, message);
    }

    public static Report Error(string message = "")
    {
        return new(Status.Error, message);
    }

    public static Report NotFound(string message = "")
    {
        return new(Status.NotFound, message);
    }
}
