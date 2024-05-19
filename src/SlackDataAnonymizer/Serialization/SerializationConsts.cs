using SlackDataAnonymizer.Serialization.Converters;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SlackDataAnonymizer.Serialization;

public static class SerializationConsts
{
    public static JsonSerializerOptions Options { get; }

    static SerializationConsts()
    {
        Options = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
        Options.Converters.Add(new OneOfTextContainerJsonConverter());
    }
}
