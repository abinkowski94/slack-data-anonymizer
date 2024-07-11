using Moq;
using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Commands;
using SlackDataAnonymizer.Models.Slack;
using SlackDataAnonymizer.Services.Anonymizers;

namespace SlackDataAnonymizer.UnitTests.Services.Anonymizers;

public class TextAnonymizerServiceTests
{
    private readonly TextAnonymizerService sut;

    public TextAnonymizerServiceTests()
    {
        sut = new();
    }

    [Fact]
    public void Anonymize_ShouldReturnNull_WhenValueIsNull()
    {
        // Arrange
        string? value = null;
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        // Act
        var result = sut.Anonymize(value, command, sensitiveData);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Anonymize_ShouldAnonymizeUserIds()
    {
        // Arrange
        const string value = "Hello <@U12345> and <@U67890>";

        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveDataMock = new Mock<ISensitiveData>();

        sensitiveDataMock
            .Setup(x => x.GetOrAddUser("U12345", It.IsAny<UserProfile>()))
            .Returns("anonymizedU12345");

        sensitiveDataMock
            .Setup(x => x.GetOrAddUser("U67890", It.IsAny<UserProfile>()))
            .Returns("anonymizedU67890");

        // Act
        var result = sut.Anonymize(value, command, sensitiveDataMock.Object);

        // Assert
        Assert.Equal("Hello <@anonymizedU12345> and <@anonymizedU67890>", result);
    }

    [Fact]
    public void Anonymize_ShouldAnonymizeTextTags()
    {
        // Arrange
        const string value = "Hello [CONFIDENTIAL] and [SECRET]";

        var command = new AnonymizeDataCommand
        {
            TextTags = ["CONFIDENTIAL", "SECRET"]
        };
        var sensitiveDataMock = new Mock<ISensitiveData>();

        sensitiveDataMock
            .Setup(x => x.GetOrAddTag("CONFIDENTIAL"))
            .Returns("anonymizedCONFIDENTIAL");

        sensitiveDataMock
            .Setup(x => x.GetOrAddTag("SECRET"))
            .Returns("anonymizedSECRET");

        // Act
        var result = sut.Anonymize(value, command, sensitiveDataMock.Object);

        // Assert
        Assert.Equal("Hello [anonymizedCONFIDENTIAL] and [anonymizedSECRET]", result);
    }

    [Fact]
    public void Anonymize_ShouldAnonymizeUserIdsAndTextTags()
    {
        // Arrange
        const string value = "Hello <@U12345> and [CONFIDENTIAL]";

        var command = new AnonymizeDataCommand
        {
            TextTags = ["CONFIDENTIAL"]
        };
        var sensitiveDataMock = new Mock<ISensitiveData>();

        sensitiveDataMock
            .Setup(x => x.GetOrAddUser("U12345", It.IsAny<UserProfile>()))
            .Returns("anonymizedU12345");

        sensitiveDataMock
            .Setup(x => x.GetOrAddTag("CONFIDENTIAL"))
            .Returns("anonymizedCONFIDENTIAL");

        // Act
        var result = sut.Anonymize(value, command, sensitiveDataMock.Object);

        // Assert
        Assert.Equal("Hello <@anonymizedU12345> and [anonymizedCONFIDENTIAL]", result);
    }
}
