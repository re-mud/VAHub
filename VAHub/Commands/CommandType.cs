using System.Text.Json.Serialization;

namespace VAHub.Commands;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CommandType
{
    Program,
    Python,
    Json
}
