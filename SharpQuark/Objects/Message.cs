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

    [JsonProperty("edited")] 
    public required bool Edited;

    [JsonProperty("attachments")]
    public required string[] AttachmentLinks; // TODO: Attachment objects with metadata

    [JsonProperty("specialAttributes")]
    public object[] SpecialAttributes = [];

    [JsonProperty("author")]
    public required User Author;
}