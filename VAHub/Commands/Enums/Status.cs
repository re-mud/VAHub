using System.Text.Json.Serialization;

namespace VAHub.Commands.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Status
{
    Success,
    Error,
    NotFound
}
