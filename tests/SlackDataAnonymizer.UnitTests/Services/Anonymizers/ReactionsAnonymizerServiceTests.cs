using Moq;
using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Commands;
using SlackDataAnonymizer.Models.Slack;
using SlackDataAnonymizer.Services.Anonymizers;

namespace SlackDataAnonymizer.UnitTests.Services.Anonymizers;

public class ReactionsAnonymizerServiceTests
{
    private readonly ReactionsAnonymizerService sut;

    public ReactionsAnonymizerServiceTests()
    {
        sut = new();
    }

    [Fact]
    public void Anonymize_ShouldReturnNull_WhenReactionIsNull()
    {
        // Arrange
        Reaction? reaction = null;
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        // Act
        var result = sut.Anonymize(reaction, command, sensitiveData);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Anonymize_ShouldReturnReaction_WhenUsersIsNull()
    {
        // Arrange
        var reaction = new Reaction { Users = null };
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        // Act
        var result = sut.Anonymize(reaction, command, sensitiveData);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Users);
    }

    [Fact]
    public void Anonymize_ShouldAnonymizeUsers_WhenUsersAreNotNull()
    {
        // Arrange
        const string originalUser1 = "user1";
        const string originalUser2 = "user2";
        const string anonymizedUser1 = "anonymizedUser1";
        const string anonymizedUser2 = "anonymizedUser2";

        var users = new[] { originalUser1, originalUser2 };
        var reaction = new Reaction { Users = users };
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveDataMock = new Mock<ISensitiveData>();

        sensitiveDataMock
            .Setup(x => x.GetOrAddUser(originalUser1, It.IsAny<UserProfile>()))
            .Returns(anonymizedUser1);

        sensitiveDataMock
            .Setup(x => x.GetOrAddUser(originalUser2, It.IsAny<UserProfile>()))
            .Returns(anonymizedUser2);

        // Act
        var result = sut.Anonymize(reaction, command, sensitiveDataMock.Object);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Users);
        Assert.Equal(new[] { anonymizedUser1, anonymizedUser2 }, result.Users);

        sensitiveDataMock.Verify(x => x.GetOrAddUser(originalUser1, It.IsAny<UserProfile>()), Times.Once);
        sensitiveDataMock.Verify(x => x.GetOrAddUser(originalUser2, It.IsAny<UserProfile>()), Times.Once);
    }
}
