using Moq;
using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Repositories.Read;
using SlackDataAnonymizer.Abstractions.Repositories.Write;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Commands;
using SlackDataAnonymizer.Models.Slack;
using SlackDataAnonymizer.Services;

namespace SlackDataAnonymizer.UnitTests.Services;

public class MessagesServiceTests
{
    private readonly Mock<IMessagesReadRepository> readRepositoryMock;
    private readonly Mock<IMessagesWriteRepository> writeRepositoryMock;
    private readonly Mock<ISensitiveDataWriteRepository> sensitiveDataRepositoryMock;
    private readonly Mock<ISlackMessageAnonymizerService> anonymizerServiceMock;
    private readonly MessagesService sut;

    public MessagesServiceTests()
    {
        readRepositoryMock = new();
        writeRepositoryMock = new();
        sensitiveDataRepositoryMock = new();
        anonymizerServiceMock = new();

        sut = new(
            readRepositoryMock.Object,
            writeRepositoryMock.Object,
            sensitiveDataRepositoryMock.Object,
            anonymizerServiceMock.Object
        );
    }

    [Fact]
    public async Task AnonymizeMessagesAsync_ShouldAnonymizeAndSaveMessages()
    {
        // Arrange
        var command = new AnonymizeDataCommand { TextTags = [] };
        var cancellationToken = CancellationToken.None;

        var messages = new List<SlackMessage>
        {
            new() { Text = "Message 1" },
            new() { Text = "Message 2" }
        };

        int? enumeratedCount = null;

        readRepositoryMock
            .Setup(x => x.GetSlackMessagesAsync(cancellationToken))
            .Returns(ToAsyncEnumerable(messages));

        writeRepositoryMock
            .Setup(x => x.CreateSlackMessagesAsync(It.IsAny<IAsyncEnumerable<SlackMessage>>(), cancellationToken))
            .Returns(ValueTask.CompletedTask)
            .Callback((IAsyncEnumerable<SlackMessage> m, CancellationToken ct) => enumeratedCount = m.ToBlockingEnumerable(ct).Count());

        sensitiveDataRepositoryMock
            .Setup(x => x.CreateSensitiveDataAsync(It.IsAny<ISensitiveData>(), cancellationToken))
            .Returns(ValueTask.CompletedTask);

        // Act
        await sut.AnonymizeMessagesAsync(command, cancellationToken);

        // Assert
        Assert.Equal(messages.Count, enumeratedCount);

        readRepositoryMock.Verify(x => x.GetSlackMessagesAsync(cancellationToken), Times.Once);
        anonymizerServiceMock.Verify(x => x.Anonymize(It.IsAny<SlackMessage>(), command, It.IsAny<ISensitiveData>()), Times.Exactly(messages.Count));
        writeRepositoryMock.Verify(x => x.CreateSlackMessagesAsync(It.IsAny<IAsyncEnumerable<SlackMessage>>(), cancellationToken), Times.Once);
        sensitiveDataRepositoryMock.Verify(x => x.CreateSensitiveDataAsync(It.IsAny<ISensitiveData>(), cancellationToken), Times.Once);
    }

    private static async IAsyncEnumerable<SlackMessage> ToAsyncEnumerable(IEnumerable<SlackMessage> messages)
    {
        foreach (var message in messages)
        {
            yield return message;
            await Task.Yield();
        }
    }
}
