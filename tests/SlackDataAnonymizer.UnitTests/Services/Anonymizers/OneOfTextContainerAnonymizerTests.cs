using Moq;
using OneOf;
using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Commands;
using SlackDataAnonymizer.Models.Slack;
using SlackDataAnonymizer.Services.Anonymizers;

namespace SlackDataAnonymizer.UnitTests.Services.Anonymizers;

public class OneOfTextContainerAnonymizerTests
{
    private readonly Mock<IAnonymizerService<TextContainer>> textContainerAnonymizerMock;
    private readonly Mock<IAnonymizerService<string>> textAnonymizerMock;

    private readonly OneOfTextContainerAnonymizer sut;

    public OneOfTextContainerAnonymizerTests()
    {
        textContainerAnonymizerMock = new();
        textAnonymizerMock = new();

        sut = new(textContainerAnonymizerMock.Object, textAnonymizerMock.Object);
    }

    [Fact]
    public void Anonymize_ShouldAnonymizeTextContainer_WhenValueIsTextContainer()
    {
        // Arrange
        var originalTextContainer = new TextContainer { Text = "originalText" };
        var anonymizedTextContainer = new TextContainer { Text = "anonymizedText" };
        var value = OneOf<TextContainer?, string?>.FromT0(originalTextContainer);
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        textContainerAnonymizerMock
            .Setup(x => x.Anonymize(originalTextContainer, command, sensitiveData))
            .Returns(anonymizedTextContainer);

        // Act
        var result = sut.Anonymize(value, command, sensitiveData);

        // Assert
        Assert.Equal(anonymizedTextContainer, result.AsT0);

        textContainerAnonymizerMock.Verify(x => x.Anonymize(originalTextContainer, command, sensitiveData), Times.Once);
    }

    [Fact]
    public void Anonymize_ShouldAnonymizeString_WhenValueIsString()
    {
        // Arrange
        const string originalText = "originalText";
        const string anonymizedText = "anonymizedText";

        var value = OneOf<TextContainer?, string?>.FromT1(originalText);
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        textAnonymizerMock
            .Setup(x => x.Anonymize(originalText, command, sensitiveData))
            .Returns(anonymizedText);

        // Act
        var result = sut.Anonymize(value, command, sensitiveData);

        // Assert
        Assert.Equal(anonymizedText, result.AsT1);

        textAnonymizerMock.Verify(x => x.Anonymize(originalText, command, sensitiveData), Times.Once);
    }
}
