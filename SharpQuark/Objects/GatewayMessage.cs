using Newtonsoft.Json;
using SharpQuark.Token;

namespace SharpQuark.Objects;

public class GatewayMessage(string @event, string? message, string? state = null)
{
    [JsonProperty("event")] public string Event = @event;
    [JsonProperty("message")] public string? Message = message;
    [JsonProperty("state")] public string? State = state;
}

// public class AuthenticateGatewayMessage(string @event, AccessToken token, string? state = null)
// {
//     [JsonProperty("eventId")] public required string Event = @event;
//     [JsonProperty("token")] public string Token = token.ToString()!;
//     [JsonProperty("state")] public string? State = state;
// }

public class AuthenticateGatewayMessage(string @event, AccessToken token, string? state = null) : GatewayMessage(@event, null, state)
{
    [JsonProperty("token")] public string Token = token.ToString()!;
}

public class GatewayEventBase
{
    [JsonProperty("event")] public required string Event;
    [JsonProperty("state")] public string? State;
}