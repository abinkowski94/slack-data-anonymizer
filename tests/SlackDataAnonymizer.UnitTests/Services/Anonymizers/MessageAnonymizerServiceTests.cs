using Moq;
using OneOf;
using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Commands;
using SlackDataAnonymizer.Models.Slack;
using SlackDataAnonymizer.Services.Anonymizers;

namespace SlackDataAnonymizer.UnitTests.Services.Anonymizers;

public class MessageAnonymizerServiceTests
{
    private readonly Mock<IAnonymizerService<OneOf<TextContainer?, string?>>> textOneOfAnonymizerMock;
    private readonly Mock<IAnonymizerService<Reply>> repliesAnonymizerMock;
    private readonly Mock<IAnonymizerService<Reaction>> reactionAnonymizerMock;
    private readonly Mock<IAnonymizerService<Block>> blocksAnonymizerMock;
    private readonly Mock<IAnonymizerService<Attachment>> attachmentAnonymizerMock;

    private readonly MessageAnonymizerService sut;

    public MessageAnonymizerServiceTests()
    {
        textOneOfAnonymizerMock = new();
        repliesAnonymizerMock = new();
        reactionAnonymizerMock = new();
        blocksAnonymizerMock = new();
        attachmentAnonymizerMock = new();

        sut = new(
            textOneOfAnonymizerMock.Object,
            repliesAnonymizerMock.Object,
            reactionAnonymizerMock.Object,
            blocksAnonymizerMock.Object,
            attachmentAnonymizerMock.Object
        );
    }

    [Fact]
    public void Anonymize_ShouldReturnNull_WhenMessageIsNull()
    {
        // Arrange
        SlackMessage? message = null;
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        // Act
        var result = sut.Anonymize(message, command, sensitiveData);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Anonymize_ShouldAnonymizeUser_WhenUserIsNotNull()
    {
        // Arrange
        const string userId = "originalUserId";
        const string anonymizedUserId = "anonymizedUserId";

        var message = new SlackMessage { User = userId };
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveDataMock = new Mock<ISensitiveData>();

        sensitiveDataMock
            .Setup(x => x.GetOrAddUser(userId, It.IsAny<UserProfile>()))
            .Returns(anonymizedUserId);

        // Act
        var result = sut.Anonymize(message, command, sensitiveDataMock.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(anonymizedUserId, result.User);
    }

    [Fact]
    public void Anonymize_ShouldAnonymizeText_WhenTextIsNotNull()
    {
        // Arrange
        var originalText = OneOf<TextContainer?, string?>.FromT1("originalText");
        var anonymizedText = OneOf<TextContainer?, string?>.FromT1("anonymizedText");
        var message = new SlackMessage { Text = originalText };
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        textOneOfAnonymizerMock
            .Setup(x => x.Anonymize(originalText, command, sensitiveData))
            .Returns(anonymizedText);

        // Act
        var result = sut.Anonymize(message, command, sensitiveData);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(anonymizedText, result.Text);

        textOneOfAnonymizerMock.Verify(x => x.Anonymize(originalText, command, sensitiveData), Times.Once);
    }

    [Fact]
    public void Anonymize_ShouldAnonymizeReplyUsers_WhenReplyUsersAreNotNull()
    {
        // Arrange
        const string replyUser1 = "replyUser1";
        const string replyUser2 = "replyUser2";
        const string anonymizedReplyUser1 = "anonymizedReplyUser1";
        const string anonymizedReplyUser2 = "anonymizedReplyUser2";

        var message = new SlackMessage { ReplyUsers = [replyUser1, replyUser2] };
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveDataMock = new Mock<ISensitiveData>();

        sensitiveDataMock
            .Setup(x => x.GetOrAddUser(replyUser1, It.IsAny<UserProfile>()))
            .Returns(anonymizedReplyUser1);

        sensitiveDataMock
            .Setup(x => x.GetOrAddUser(replyUser2, It.IsAny<UserProfile>()))
            .Returns(anonymizedReplyUser2);

        // Act
        var result = sut.Anonymize(message, command, sensitiveDataMock.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(new[] { anonymizedReplyUser1, anonymizedReplyUser2 }, result.ReplyUsers);
    }

    [Fact]
    public void Anonymize_ShouldAnonymizeParentUserId_WhenParentUserIdIsNotNull()
    {
        // Arrange
        const string parentUserId = "parentUserId";
        const string anonymizedParentUserId = "anonymizedParentUserId";

        var message = new SlackMessage { ParentUserId = parentUserId };
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveDataMock = new Mock<ISensitiveData>();

        sensitiveDataMock
            .Setup(x => x.GetOrAddUser(parentUserId, It.IsAny<UserProfile>()))
            .Returns(anonymizedParentUserId);

        // Act
        var result = sut.Anonymize(message, command, sensitiveDataMock.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(anonymizedParentUserId, result.ParentUserId);
    }

    [Fact]
    public void Anonymize_ShouldAnonymizeReplies_WhenRepliesAreNotNull()
    {
        // Arrange
        var message = new SlackMessage { Replies = [new Reply(), new Reply()] };
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        // Act
        var result = sut.Anonymize(message, command, sensitiveData);

        // Assert
        Assert.NotNull(result);

        repliesAnonymizerMock.Verify(x => x.Anonymize(message.Replies, command, sensitiveData), Times.Once);
    }

    [Fact]
    public void Anonymize_ShouldAnonymizeReactions_WhenReactionsAreNotNull()
    {
        // Arrange
        var message = new SlackMessage { Reactions = [new Reaction(), new Reaction()] };
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        // Act
        var result = sut.Anonymize(message, command, sensitiveData);

        // Assert
        Assert.NotNull(result);

        reactionAnonymizerMock.Verify(x => x.Anonymize(message.Reactions, command, sensitiveData), Times.Once);
    }

    [Fact]
    public void Anonymize_ShouldAnonymizeBlocks_WhenBlocksAreNotNull()
    {
        // Arrange
        var message = new SlackMessage { Blocks = [new Block(), new Block()] };
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        // Act
        var result = sut.Anonymize(message, command, sensitiveData);

        // Assert
        Assert.NotNull(result);

        blocksAnonymizerMock.Verify(x => x.Anonymize(message.Blocks, command, sensitiveData), Times.Once);
    }

    [Fact]
    public void Anonymize_ShouldAnonymizeAttachments_WhenAttachmentsAreNotNull()
    {
        // Arrange
        var message = new SlackMessage { Attachments = [new Attachment(), new Attachment()] };
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        // Act
        var result = sut.Anonymize(message, command, sensitiveData);

        // Assert
        Assert.NotNull(result);

        attachmentAnonymizerMock.Verify(x => x.Anonymize(message.Attachments, command, sensitiveData), Times.Once);
    }
}
