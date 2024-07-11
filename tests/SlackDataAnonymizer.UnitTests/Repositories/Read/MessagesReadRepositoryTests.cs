using SlackDataAnonymizer.Exceptions;
using SlackDataAnonymizer.Models.Slack;
using SlackDataAnonymizer.Repositories.Read;
using SlackDataAnonymizer.Serialization;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace SlackDataAnonymizer.UnitTests.Repositories.Read;

public class MessagesReadRepositoryTests
{
    private string messagesPath = "";
    private readonly JsonSerializerOptions options = SerializationConsts.Options;

    private MessagesReadRepository Sut => new(messagesPath, options);

    [Fact]
    public async Task GetSlackMessagesAsync_ShouldReturnMessages_WhenFileIsValid()
    {
        // Arrange
        SetupMessagePath();

        var messages = new List<SlackMessage>
        {
            new() { Text = "Message 1" },
            new() { Text = "Message 2" }
        };

        var json = JsonSerializer.Serialize(messages, options);

        await File.WriteAllTextAsync(messagesPath, json);

        var cancellationToken = CancellationToken.None;

        // Act
        var result = Sut.GetSlackMessagesAsync(cancellationToken).ToBlockingEnumerable().ToList();

        // Assert
        Assert.Equal(messages.Count, result.Count);
        Assert.Equal(messages[0].Text, result[0].Text);
        Assert.Equal(messages[1].Text, result[1].Text);

        // Clean up
        File.Delete(messagesPath);
    }

    [Fact]
    public async Task GetSlackMessagesAsync_ShouldThrowFileReadingException_WhenFileReadingFails()
    {
        // Arrange
        SetupMessagePath();

        var invalidMessagesPath = "invalid-path.json";
        var invalidSut = new MessagesReadRepository(invalidMessagesPath, options);
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<FileReadingException>(async () =>
        {
            await foreach (var _ in invalidSut.GetSlackMessagesAsync(cancellationToken))
            {
            }
        });

        Assert.Equal(invalidMessagesPath, exception.FilePath);
    }

    [Fact]
    public async Task GetSlackMessagesAsync_ShouldHandleEmptyFileGracefully()
    {
        // Arrange
        SetupMessagePath();

        await File.WriteAllTextAsync(messagesPath, string.Empty);

        var cancellationToken = CancellationToken.None;

        // Act
        var result = Sut.GetSlackMessagesAsync(cancellationToken).ToBlockingEnumerable().ToList();

        // Assert
        Assert.Empty(result);

        // Clean up
        File.Delete(messagesPath);
    }

    [Fact]
    public async Task GetSlackMessagesAsync_ShouldHandleEmptyJsonGracefully()
    {
        // Arrange
        SetupMessagePath();

        await File.WriteAllTextAsync(messagesPath, """[]""");

        var cancellationToken = CancellationToken.None;

        // Act
        var result = Sut.GetSlackMessagesAsync(cancellationToken).ToBlockingEnumerable().ToList();

        // Assert
        Assert.Empty(result);

        // Clean up
        File.Delete(messagesPath);
    }

    private void SetupMessagePath([CallerMemberName] string? methodName = null)
    {
        messagesPath = $"{methodName}-MessagesReadRepositoryTests-WorkFile.json";
    }
}
