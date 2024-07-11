using Moq;
using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Service;
using SlackDataAnonymizer.Commands;
using SlackDataAnonymizer.Models.Slack;
using SlackDataAnonymizer.Services.Anonymizers;

namespace SlackDataAnonymizer.UnitTests.Services.Anonymizers;

public class BlocksAnonymizerServiceTests
{
    private readonly Mock<IAnonymizerService<Element>> elementsAnonymizerMock;
    private readonly BlocksAnonymizerService sut;

    public BlocksAnonymizerServiceTests()
    {
        elementsAnonymizerMock = new();
        sut = new(elementsAnonymizerMock.Object);
    }

    [Fact]
    public void Anonymize_ShouldReturnNull_WhenBlockIsNull()
    {
        // Arrange
        Block? block = null;
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        // Act
        var result = sut.Anonymize(block, command, sensitiveData);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Anonymize_ShouldReturnBlock_WhenElementsAreNull()
    {
        // Arrange
        var block = new Block { Elements = null };
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        // Act
        var result = sut.Anonymize(block, command, sensitiveData);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Elements);

        elementsAnonymizerMock.Verify(x => x.Anonymize(It.IsAny<Element>(), It.IsAny<AnonymizeDataCommand>(), It.IsAny<ISensitiveData>()), Times.Never);
    }

    [Fact]
    public void Anonymize_ShouldAnonymizeElements_WhenElementsAreNotNull()
    {
        // Arrange
        var element1 = new Element();
        var element2 = new Element();
        var block = new Block { Elements = [element1, element2] };
        var command = new AnonymizeDataCommand { TextTags = [] };
        var sensitiveData = Mock.Of<ISensitiveData>();

        // Act
        var result = sut.Anonymize(block, command, sensitiveData);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Elements);

        elementsAnonymizerMock.Verify(x => x.Anonymize(element1, command, sensitiveData), Times.Once);
        elementsAnonymizerMock.Verify(x => x.Anonymize(element2, command, sensitiveData), Times.Once);
    }
}
