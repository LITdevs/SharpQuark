using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpQuark.Objects.Id;

namespace SharpQuark.Objects;

public class ChannelConverter : JsonConverter
{
    private static readonly Dictionary<ChannelId, Channel> Instances = new();

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Channel);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        var channelId = jsonObject.GetValue("_id")?.ToObject<ChannelId>();
                
        if (Instances.TryGetValue(channelId ?? throw new Exception("Channel ID null"), out var channel))
            return channel;

        // Deserialize other properties
        var name = jsonObject.GetValue("name")?.ToObject<string>();
        var quarkId = jsonObject.GetValue("quark")?.ToObject<QuarkId>();
        var description = jsonObject.GetValue("description")?.ToObject<string>();

        // Create new Channel instance
        channel = new Channel(channelId, name ?? throw new Exception("Channel Name null"), quarkId ?? throw new Exception("Channel Quark ID null"), description);
        Instances[channelId] = channel;
        return channel;
    }
}

[JsonConverter(typeof(ChannelConverter))]
public class Channel
{
    [JsonProperty("_id")]
    public ChannelId Id;
    [JsonProperty("name")]
    public string Name;
    [JsonProperty("description")]
    public string Description;
    [JsonProperty("quark")]
    public QuarkId QuarkId;
    [JsonProperty("messages")] 
    public SortedSet<Message> Messages;

    [JsonIgnore] public Lightquark Lq = null!;

    [JsonIgnore] private bool _initialLoad;

    internal Channel(ChannelId id, string name, QuarkId quarkId, string? description)
    {
        Id = id;
        Name = name;
        QuarkId = quarkId;
        Description = description ?? string.Empty;
        Messages = new SortedSet<Message>(new TimestampComparer());
    }
        
    private async Task<Message[]> GetMessages()
    {
        return (await Lq.ChannelMessages(this)).Response.Messages;
    }

    public async Task InitialLoad()
    {
        if (_initialLoad)
        {
            Debug.WriteLine($"[c{Id}] Initial loaded before, skipping");
            return;
        }
        Messages.UnionWith(await GetMessages());
        _initialLoad = true;
    }
};