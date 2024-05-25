using Newtonsoft.Json;

namespace SharpQuark.Objects.Id;

[JsonConverter(typeof(QuarkIdConverter))]
public class QuarkId(string id) : BaseId(id)
{
    public override bool Equals(object? obj)
    {
        if (obj is QuarkId other)
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

public class QuarkIdConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(QuarkId);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        {
            return new QuarkId((string?)reader.Value ?? string.Empty);
        }
        throw new JsonSerializationException($"Unexpected token type. ({reader.TokenType})");
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        value ??= string.Empty;
        var id = (QuarkId)value;
        writer.WriteValue(id.Id);
    }
}