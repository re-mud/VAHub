using System.Text.Json.Serialization;
using VAHub.Commands;

namespace VAHub.Models;

public class Manifest
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public CommandType Type { get; set; } = CommandType.None;

    [JsonPropertyName("commands")]
    public Dictionary<string, string> Commands { get; set; } = [];
}
