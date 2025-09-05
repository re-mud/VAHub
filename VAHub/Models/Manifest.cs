using System.Text.Json.Serialization;
using VAHub.Commands;

namespace VAHub.Models;

public class Manifest
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("version")]
    public required string Version { get; set; }

    [JsonPropertyName("type")]
    public required CommandType Type { get; set; }

    [JsonPropertyName("commands")]
    public required Dictionary<string, string> Commands { get; set; }
}
