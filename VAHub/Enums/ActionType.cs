using System.Text.Json.Serialization;

namespace VAHub.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ActionType
{
    None,
    Close
}
