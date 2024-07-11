using Moq;
using SlackDataAnonymizer.Abstractions.Repositories.Read;
using SlackDataAnonymizer.Models.Slack;
using SlackDataAnonymizer.Repositories.Read;

namespace SlackDataAnonymizer.UnitTests.Repositories.Read;

public class CompositeMessagesReadRepositoryTests
{
    private readonly CompositeMessagesReadRepository sut;
    private readonly Mock<IMessagesReadRepository> readRepositoryMock1;
    private readonly Mock<IMessagesReadRepository> readRepositoryMock2;

    public CompositeMessagesReadRepositoryTests()
    {
        readRepositoryMock1 = new Mock<IMessagesReadRepository>();
        readRepositoryMock2 = new Mock<IMessagesReadRepository>();

        var readRepositories = new List<IMessagesReadRepository>
            {
                readRepositoryMock1.Object,
                readRepositoryMock2.Object
            };

        sut = new CompositeMessagesReadRepository(readRepositories);
    }

    [Fact]
    public void GetSlackMessagesAsync_ShouldReturnMessagesFromAllRepositories()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var messages1 = new List<SlackMessage>
        {
            new() { Text = "Message 1 from repo 1" },
            new() { Text = "Message 2 from repo 1" }
        };

        var messages2 = new List<SlackMessage>
        {
            new() { Text = "Message 1 from repo 2" },
            new() { Text = "Message 2 from repo 2" }
        };

        readRepositoryMock1
            .Setup(x => x.GetSlackMessagesAsync(cancellationToken))
            .Returns(ToAsyncEnumerable(messages1));

        readRepositoryMock2
            .Setup(x => x.GetSlackMessagesAsync(cancellationToken))
            .Returns(ToAsyncEnumerable(messages2));

        // Act
        var result = sut.GetSlackMessagesAsync(cancellationToken).ToBlockingEnumerable().ToList();

        // Assert
        Assert.Equal(4, result.Count);
        Assert.Contains(result, m => m.Text.AsT1 == "Message 1 from repo 1");
        Assert.Contains(result, m => m.Text.AsT1 == "Message 2 from repo 1");
        Assert.Contains(result, m => m.Text.AsT1 == "Message 1 from repo 2");
        Assert.Contains(result, m => m.Text.AsT1 == "Message 2 from repo 2");
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
