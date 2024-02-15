using Newtonsoft.Json;

namespace SharpQuark.Objects.Id;

[JsonConverter(typeof(UserIdConverter))]
public class UserId(string id) : BaseId(id)
{
    public override bool Equals(object? obj)
    {
        if (obj is UserId other)
        {
            return Id == other.Id;
        }
        return false;
    }
    
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}

public class UserIdConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(UserId);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        {
            return new UserId((string?)reader.Value ?? string.Empty);
        }

        throw new JsonSerializationException("Unexpected token type.");
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        value ??= string.Empty;
        var id = (UserId)value;
        writer.WriteValue(id.Id);
    }
}