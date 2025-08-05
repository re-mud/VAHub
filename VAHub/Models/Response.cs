using System.Text.Json.Serialization;
using VAHub.Enums;

namespace VAHub.Models;

public class Response
{
    public Response(
        Status status = default, 
        string message = "",
        string speakText = "")
    {
        Status = status;
        Message = message;
        SpeakText = speakText;
    }

    [JsonPropertyName("status")]
    public Status Status { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("speak_text")]
    public string SpeakText { get; set; }
}
