using Moq;
using OneOf;
using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Commands;
using SlackDataAnonymizer.Models.Slack;
using SlackDataAnonymizer.Services.Anonymizers;

namespace SlackDataAnonymizer.UnitTests.Services.Anonymizers;

public class AttachmentsAnonymizerServiceTests
{
    private readonly Mock<IAnonymizerService<OneOf<TextContainer?, string?>>> textOneOfAnonymizerMock;
    private readonly AttachmentsAnonymizerService sut;

    public AttachmentsAnonymizerServiceTests()
    {
        textOneOfAnonymizerMock = new();
        sut = new(textOneOfAnonymizerMock.Object);
    }

    [Fact]
    public void Anonymize_ShouldReturnNull_WhenAttachmentIsNull()
    {
        // Arrange
        Attachment? attachment = null;
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        // Act
        var result = sut.Anonymize(attachment, command, sensitiveData);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Anonymize_ShouldAnonymizeText_WhenAttachmentIsNotNull()
    {
        // Arrange
        const string originalText = "Sensitive text";
        const string anonymizedText = "Anonymized text";

        var textContainer = OneOf<TextContainer?, string?>.FromT1(originalText);
        var attachment = new Attachment { Text = textContainer };
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        textOneOfAnonymizerMock
            .Setup(x => x.Anonymize(It.IsAny<OneOf<TextContainer?, string?>>(), It.IsAny<AnonymizeDataCommand>(), It.IsAny<ISensitiveData>()))
            .Returns(OneOf<TextContainer?, string?>.FromT1(anonymizedText));

        // Act
        var result = sut.Anonymize(attachment, command, sensitiveData);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(anonymizedText, result.Text.AsT1);

        textOneOfAnonymizerMock.Verify(x => x.Anonymize(textContainer, command, sensitiveData), Times.Once);
    }
}
