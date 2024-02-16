using Newtonsoft.Json;

namespace SharpQuark.Objects;

public struct GatewayMessage(string @event, string message, string? state = null)
{
    [JsonProperty("event")] public string Event = @event;
    [JsonProperty("message")] public string Message = message;
    [JsonProperty("state")] public string? State = state;
}

public class GatewayEventBase
{
    [JsonProperty("eventId")] public required string Event;
    [JsonProperty("state")] public string? State;
}