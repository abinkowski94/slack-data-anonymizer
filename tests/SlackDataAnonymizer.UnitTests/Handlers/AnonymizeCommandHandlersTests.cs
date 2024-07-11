using Moq;
using SlackDataAnonymizer.Abstractions.Factories;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Commands;
using SlackDataAnonymizer.Handlers;

namespace SlackDataAnonymizer.UnitTests.Handlers;

public class AnonymizeCommandHandlersTests
{
    private readonly Mock<IMessagesServiceFactory> messagesServiceFactoryMock;
    private readonly AnonymizeCommandHandlers sut;

    public AnonymizeCommandHandlersTests()
    {
        messagesServiceFactoryMock = new();
        sut = new(messagesServiceFactoryMock.Object);
    }

    [Fact]
    public async Task AnonymizeAsync_ShouldCallAnonymizeMessagesAsync()
    {
        // Arrange
        var consoleCommand = new AnonymizeConsoleCommand
        {
            TextTags = [ "CONFIDENTIAL", "SECRET" ],
            SourceDirectory = "."
        };
        var cancellationToken = CancellationToken.None;

        var messagesServiceMock = new Mock<IMessagesService>();

        messagesServiceFactoryMock
            .Setup(x => x.Create(consoleCommand))
            .Returns(messagesServiceMock.Object);

        messagesServiceMock
            .Setup(x => x.AnonymizeMessagesAsync(
                It.Is<AnonymizeDataCommand>(c => c.TextTags.SequenceEqual(consoleCommand.TextTags)),
                cancellationToken))
            .Returns(ValueTask.CompletedTask);

        // Act
        await sut.AnonymizeAsync(consoleCommand, cancellationToken);

        // Assert
        messagesServiceFactoryMock.Verify(x => x.Create(consoleCommand), Times.Once);

        messagesServiceMock.Verify(x => x.AnonymizeMessagesAsync(
            It.Is<AnonymizeDataCommand>(c => c.TextTags.SequenceEqual(consoleCommand.TextTags)),
            cancellationToken),
            Times.Once);
    }
}
