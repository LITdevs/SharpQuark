using Newtonsoft.Json;
using SharpQuark.Objects.Id;

namespace SharpQuark.Objects;

public class Message
{
    [JsonProperty("_id")] 
    public required MessageId Id;
    
    [JsonProperty("authorId")] 
    public required UserId AuthorId;
    
    [JsonProperty("content")]
    public string Content = string.Empty;
    
    [JsonProperty("ua")]
    public string Agent = "Unknown";

    [JsonProperty("timestamp")] 
    public required long JsTimestamp;
    
    [JsonIgnore]
    public DateTimeOffset Timestamp => DateTimeOffset.FromUnixTimeMilliseconds(JsTimestamp);

    [JsonProperty("edited")] 
    public required bool Edited;

    [JsonProperty("attachments")]
    public required string[] AttachmentLinks; // TODO: Attachment objects with metadata

    [JsonProperty("specialAttributes")]
    public object[] SpecialAttributes = [];

    [JsonProperty("author")]
    public required User Author;

    [JsonIgnore] public Channel Channel = null!;

}


public class TimestampComparer : IComparer<Message>
{
    public int Compare(Message? x, Message? y)
    {
        if (x == null || y == null)
        {
            throw new ArgumentException("Messages cannot be null");
        }

        // First, compare by JsTimestamp
        var timestampComparison = x.JsTimestamp.CompareTo(y.JsTimestamp);
        return timestampComparison != 0 ? timestampComparison : x.Id.CompareTo(y.Id);
    }
}