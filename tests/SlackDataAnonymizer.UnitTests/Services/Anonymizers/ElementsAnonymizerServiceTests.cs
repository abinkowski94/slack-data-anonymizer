using Moq;
using OneOf;
using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Commands;
using SlackDataAnonymizer.Models.Slack;
using SlackDataAnonymizer.Services.Anonymizers;

namespace SlackDataAnonymizer.UnitTests.Services.Anonymizers;

public class ElementsAnonymizerServiceTests
{
    private readonly Mock<IAnonymizerService<OneOf<TextContainer?, string?>>> textOneOfAnonymizerMock;
    private readonly ElementsAnonymizerService sut;

    public ElementsAnonymizerServiceTests()
    {
        textOneOfAnonymizerMock = new();
        sut = new(textOneOfAnonymizerMock.Object);
    }

    [Fact]
    public void Anonymize_ShouldReturnNull_WhenElementIsNull()
    {
        // Arrange
        Element? element = null;
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        // Act
        var result = sut.Anonymize(element, command, sensitiveData);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Anonymize_ShouldAnonymizeUserId_WhenUserIdIsNotNull()
    {
        // Arrange
        const string userId = "originalUserId";
        const string anonymizedUserId = "anonymizedUserId";

        var element = new Element { UserId = userId };
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveDataMock = new Mock<ISensitiveData>();

        sensitiveDataMock
            .Setup(x => x.GetOrAddUser(userId, It.IsAny<UserProfile>()))
            .Returns(anonymizedUserId);

        // Act
        var result = sut.Anonymize(element, command, sensitiveDataMock.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(anonymizedUserId, result.UserId);
    }

    [Fact]
    public void Anonymize_ShouldAnonymizeText_WhenTextIsNotNull()
    {
        // Arrange
        var originalText = OneOf<TextContainer?, string?>.FromT1("originalText");
        var anonymizedText = OneOf<TextContainer?, string?>.FromT1("anonymizedText");
        var element = new Element { Text = originalText };
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        textOneOfAnonymizerMock
            .Setup(x => x.Anonymize(originalText, command, sensitiveData))
            .Returns(anonymizedText);

        // Act
        var result = sut.Anonymize(element, command, sensitiveData);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(anonymizedText, result.Text);

        textOneOfAnonymizerMock.Verify(x => x.Anonymize(originalText, command, sensitiveData), Times.Once);
    }

    [Fact]
    public void Anonymize_ShouldAnonymizeSubElements_WhenSubElementsAreNotNull()
    {
        // Arrange
        var subElement1 = new Element { Text = "text one" };
        var subElement2 = new Element { Text = "test two" };
        var element = new Element { Text = "Text root", Elements = [subElement1, subElement2] };
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        // Act
        var result = sut.Anonymize(element, command, sensitiveData);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Elements);
        Assert.Equal(2, result.Elements.Length);

        textOneOfAnonymizerMock.Verify(x => x.Anonymize(
            It.IsAny<OneOf<TextContainer?, string?>>(),
            It.IsAny<AnonymizeDataCommand>(),
            It.IsAny<ISensitiveData>()),
            Times.Exactly(3));
    }
}
