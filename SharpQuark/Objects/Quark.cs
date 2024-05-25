using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpQuark.Objects.Id;

namespace SharpQuark.Objects;

public class QuarkConverter : JsonConverter
{
    private static readonly Dictionary<QuarkId, Quark> Instances = new();

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Quark);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        var quarkId = jsonObject.GetValue("_id")?.ToObject<QuarkId>();
                
        if (Instances.TryGetValue(quarkId ?? throw new Exception("Quark ID null"), out var quark))
            return quark;

        // Deserialize other properties
        var name = jsonObject.GetValue("name")?.ToObject<string>();
        var iconUri = jsonObject.GetValue("iconUri")?.ToObject<Uri>();
        var invite = jsonObject.GetValue("invite")?.ToObject<string>();
        var owners = jsonObject.GetValue("owners")?.ToObject<UserId[]>();

        // Create new Channel instance
        quark = new Quark(quarkId, 
            name ?? throw new Exception("Quark Name null"), 
            iconUri ?? throw new Exception("Quark Icon null"),
            invite ?? throw new Exception("Quark Invite null"), 
            owners ?? throw new Exception("Quark Owners null"));
        Instances[quarkId] = quark;
        return quark;
    }
}

[JsonConverter(typeof(QuarkConverter))]
public class Quark
{
    [JsonProperty("_id")]
    public QuarkId Id;
    [JsonProperty("name")]
    public string Name;
    [JsonProperty("iconUri")]
    public Uri Icon;
    [JsonProperty("invite")] 
    public string Invite;
    [JsonProperty("owners")] 
    public UserId[] Owners;
    // [JsonProperty("channels")] 
    // public Channel[] Channels;

    [JsonIgnore] public Lightquark Lq = null!;

    [JsonIgnore] private bool _initialLoad;

    internal Quark(QuarkId id, string name, Uri iconUri, string invite, UserId[] owners)
    {
        Id = id;
        Name = name;
        Icon = iconUri;
        Invite = invite;
        Owners = owners;
        // Channels = [];
    }
        
    // private async Task<Channel[]> GetChannels()
    // {
    //     return (await Lq.QuarkChannels(this)).Response.Messages;
    // }

    public async Task InitialLoad()
    {
        if (_initialLoad)
        {
            Debug.WriteLine($"[q{Id}] Initial loaded before, skipping");
            return;
        }
        // TODO: get channels
        _initialLoad = true;
    }
};