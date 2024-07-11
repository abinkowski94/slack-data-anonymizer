using Moq;
using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Commands;
using SlackDataAnonymizer.Models.Slack;
using SlackDataAnonymizer.Services.Anonymizers;

namespace SlackDataAnonymizer.UnitTests.Services.Anonymizers;

public class TextContainerAnonymizerTests
{
    private readonly TextContainerAnonymizer sut;
    private readonly Mock<IAnonymizerService<string>> textAnonymizerMock;

    public TextContainerAnonymizerTests()
    {
        textAnonymizerMock = new();
        sut = new(textAnonymizerMock.Object);
    }

    [Fact]
    public void Anonymize_ShouldReturnNull_WhenTextContainerIsNull()
    {
        // Arrange
        TextContainer? textContainer = null;
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        // Act
        var result = sut.Anonymize(textContainer, command, sensitiveData);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Anonymize_ShouldAnonymizeText_WhenTextContainerIsNotNull()
    {
        // Arrange
        const string originalText = "originalText";
        const string anonymizedText = "anonymizedText";

        var textContainer = new TextContainer { Text = originalText };
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        textAnonymizerMock
            .Setup(x => x.Anonymize(originalText, command, sensitiveData))
            .Returns(anonymizedText);

        // Act
        var result = sut.Anonymize(textContainer, command, sensitiveData);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(anonymizedText, result.Text);

        textAnonymizerMock.Verify(x => x.Anonymize(originalText, command, sensitiveData), Times.Once);
    }
}
