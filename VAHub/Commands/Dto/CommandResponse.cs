using System.Text.Json.Serialization;
using VAHub.Commands.Enums;

namespace VAHub.Commands.DTO;

public class CommandResponse
{
    [JsonPropertyName("action")]
    public ActionType Action { get; set; } = ActionType.None;

    [JsonPropertyName("speak")]
    public string Speak { get; set; } = string.Empty;

    [JsonPropertyName("context")]
    public string Context { get; set; } = string.Empty;
}
