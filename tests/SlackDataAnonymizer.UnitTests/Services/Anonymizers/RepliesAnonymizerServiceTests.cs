using Moq;
using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Commands;
using SlackDataAnonymizer.Models.Slack;
using SlackDataAnonymizer.Services.Anonymizers;

namespace SlackDataAnonymizer.UnitTests.Services.Anonymizers;

public class RepliesAnonymizerServiceTests
{
    private readonly RepliesAnonymizerService sut;

    public RepliesAnonymizerServiceTests()
    {
        sut = new();
    }

    [Fact]
    public void Anonymize_ShouldReturnNull_WhenReplyIsNull()
    {
        // Arrange
        Reply? reply = null;
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        // Act
        var result = sut.Anonymize(reply, command, sensitiveData);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Anonymize_ShouldReturnReply_WhenUserIsNull()
    {
        // Arrange
        var reply = new Reply { User = null };
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        // Act
        var result = sut.Anonymize(reply, command, sensitiveData);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.User);
    }

    [Fact]
    public void Anonymize_ShouldAnonymizeUser_WhenUserIsNotNull()
    {
        // Arrange
        var originalUser = "originalUser";
        var anonymizedUser = "anonymizedUser";
        var reply = new Reply { User = originalUser };
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveDataMock = new Mock<ISensitiveData>();

        sensitiveDataMock
            .Setup(x => x.GetOrAddUser(originalUser, It.IsAny<UserProfile>()))
            .Returns(anonymizedUser);

        // Act
        var result = sut.Anonymize(reply, command, sensitiveDataMock.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(anonymizedUser, result.User);

        sensitiveDataMock.Verify(x => x.GetOrAddUser(originalUser, It.IsAny<UserProfile>()), Times.Once);
    }
}
