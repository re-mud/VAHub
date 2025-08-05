using System.Text.Json.Serialization;
using VAHub.Enums;

namespace VAHub.Models;

public class Response
{
    [JsonPropertyName("status")]
    public Status Status { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("speak_text")]
    public string SpeakText { get; set; } = string.Empty;
}
