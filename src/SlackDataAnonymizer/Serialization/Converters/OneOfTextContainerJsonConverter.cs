using OneOf;
using SlackDataAnonymizer.Models.Slack;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SlackDataAnonymizer.Serialization.Converters;

public class OneOfTextContainerJsonConverter : JsonConverter<OneOf<TextContainer?, string?>>
{
    public override OneOf<TextContainer?, string?> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            return JsonSerializer.Deserialize<TextContainer>(ref reader, options);
        }

        return reader.GetString();
    }

    public override void Write(Utf8JsonWriter writer, OneOf<TextContainer?, string?> value, JsonSerializerOptions options)
    {
        value.Switch(
            c => JsonSerializer.Serialize(writer, c, options),
            s => JsonSerializer.Serialize(writer, s, options)
        );
    }
}
