using System.Text.Json.Serialization;
using VAHub.Enums;

namespace VAHub.Models;

public class Response
{
    [JsonPropertyName("action")]
    public ActionType Action { get; set; } = ActionType.None;

    [JsonPropertyName("speak")]
    public string Speak { get; set; } = string.Empty;
}
