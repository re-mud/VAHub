using System.Text.Json.Serialization;
using VAHub.Enums;

namespace VAHub.Models;

public class Response
{
    public Response(
        Status status = default, 
        string message = "",
        ActionType action = ActionType.None,
        string data = "")
    {
        Status = status;
        Message = message;
        Action = action;
        Data = data;
    }

    [JsonPropertyName("status")]
    public Status Status { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("action")]
    public ActionType Action { get; set; }

    [JsonPropertyName("data")]
    public string Data { get; set; }
}
