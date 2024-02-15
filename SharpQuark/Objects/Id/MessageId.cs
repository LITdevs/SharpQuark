using Newtonsoft.Json;

namespace SharpQuark.Objects.Id;

[JsonConverter(typeof(MessageIdConverter))]
public class MessageId(string id) : BaseId(id)
{
    public override bool Equals(object? obj)
    {
        if (obj is MessageId other)
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

public class MessageIdConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(MessageId);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        {
            return new MessageId((string?)reader.Value ?? string.Empty);
        }

        throw new JsonSerializationException("Unexpected token type.");
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        value ??= string.Empty;
        var id = (MessageId)value;
        writer.WriteValue(id.Id);
    }
}