using System.Text.Json.Serialization;
using VAHub.Enums;

namespace VAHub.Models;

public class Response
{
    public Response(
        Status status = default, 
        string message = "",
        ActionType action = ActionType.None,
        string speak = "")
    {
        Status = status;
        Message = message;
        Action = action;
        Speak = speak;
    }

    [JsonPropertyName("status")]
    public Status Status { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("action")]
    public ActionType Action { get; set; }

    [JsonPropertyName("speak")]
    public string Speak { get; set; }
}
