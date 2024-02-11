using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SharpQuark.Objects;

public class User
{
    [JsonProperty("isBot")]
    public bool IsBot;
    [JsonProperty("_id")]
    public required string Id;
    [JsonProperty("username")]
    public required string Username;
    [JsonProperty("email")]
    public string? Email;
    [JsonProperty("admin")]
    public bool Admin;
    [JsonProperty("status")]
    public UserStatus? Status;
    [JsonProperty("avatarUri")]
    public required string AvatarUri;

}

public class UserStatus
{
    [JsonProperty("type")]
    [JsonConverter(typeof(StringEnumConverter))]
    public required UserStatusType Type;
    [JsonProperty("content")]
    public required string Content;
    [JsonProperty("primaryImage")]
    public string? PrimaryImageUri;
    [JsonProperty("secondaryImage")]
    public string? SecondaryImageUri;
    [JsonProperty("primaryLink")]
    public string? PrimaryLink;
    [JsonProperty("secondaryLink")]
    public string? SecondaryLink;
    [JsonProperty("startTime")]
    public DateTimeOffset? StartTime;
    [JsonProperty("endTime")]
    public DateTimeOffset? EndTime;
    [JsonProperty("paused")]
    public bool? Paused;
}

public enum UserStatusType
{
    [EnumMember(Value = "playing")]
    Playing = 0,
    [EnumMember(Value = "watching")]
    Watching = 1,
    [EnumMember(Value = "listening")]
    Listening = 2,
    [EnumMember(Value = "plain")]
    Plain = 3
}