﻿using Newtonsoft.Json;

namespace SharpQuark.Objects.Id;

[JsonConverter(typeof(QuarkIdConverter))]
public class QuarkId(string id) : BaseId(id);

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
        throw new JsonSerializationException("Unexpected token type.");
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        value ??= string.Empty;
        var id = (QuarkId)value;
        writer.WriteValue(id.Id);
    }
}