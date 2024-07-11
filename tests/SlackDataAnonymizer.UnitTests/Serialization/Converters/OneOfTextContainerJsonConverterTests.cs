using OneOf;
using SlackDataAnonymizer.Models.Slack;
using SlackDataAnonymizer.Serialization.Converters;
using System.Text;
using System.Text.Json;

namespace SlackDataAnonymizer.UnitTests.Serialization.Converters;

public class OneOfTextContainerJsonConverterTests
{
    private readonly JsonSerializerOptions options;
    private readonly OneOfTextContainerJsonConverter sut;

    public OneOfTextContainerJsonConverterTests()
    {
        sut = new();
        options = new()
        {
            Converters = { sut }
        };
    }

    [Fact]
    public void Read_ShouldDeserializeTextContainer()
    {
        // Arrange
        var json = """
        {
            "text": "Some text"
        }
        """;
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
        reader.Read();

        // Act
        var result = sut.Read(ref reader, typeof(OneOf<TextContainer?, string?>), options);

        // Assert
        Assert.True(result.IsT0);
        Assert.NotNull(result.AsT0);
        Assert.Equal("Some text", result.AsT0.Text);
    }

    [Fact]
    public void Read_ShouldDeserializeString()
    {
        // Arrange
        var json = "\"Some text\"";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
        reader.Read();

        // Act
        var result = sut.Read(ref reader, typeof(OneOf<TextContainer?, string?>), options);

        // Assert
        Assert.True(result.IsT1);
        Assert.Equal("Some text", result.AsT1);
    }

    [Fact]
    public void Write_ShouldSerializeTextContainer()
    {
        // Arrange
        var textContainer = new TextContainer { Text = "Some text" };
        var value = OneOf<TextContainer?, string?>.FromT0(textContainer);

        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);

        // Act
        sut.Write(writer, value, options);
        writer.Flush();
        var json = Encoding.UTF8.GetString(stream.ToArray());

        // Assert
        var expectedJson = JsonSerializer.Serialize(textContainer, options);
        Assert.Equal(expectedJson, json);
    }

    [Fact]
    public void Write_ShouldSerializeString()
    {
        // Arrange
        var value = OneOf<TextContainer?, string?>.FromT1("Some text");

        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);

        // Act
        sut.Write(writer, value, options);
        writer.Flush();
        var json = Encoding.UTF8.GetString(stream.ToArray());

        // Assert
        var expectedJson = JsonSerializer.Serialize("Some text", options);
        Assert.Equal(expectedJson, json);
    }
}
