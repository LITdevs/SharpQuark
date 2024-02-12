using Newtonsoft.Json;
using SharpQuark.Objects.Id;

namespace SharpQuark.Objects;

public class Channel
{
    [JsonProperty("_id")]
    public required ChannelId Id;
    [JsonProperty("name")]
    public required string Name;
    [JsonProperty("description")]
    public string Description = string.Empty;
    [JsonProperty("quark")]
    public required QuarkId QuarkId;

    [JsonIgnore] public Lightquark? Lq;

    public async Task<Message[]> GetMessages()
    {
        return (await Lq.ChannelMessages(Id)).Response.Messages;
    }
};