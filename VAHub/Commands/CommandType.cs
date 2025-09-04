using System.Text.Json.Serialization;

namespace VAHub.Commands;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CommandType
{
    None,
    Program,
    Python,
    Json
}
