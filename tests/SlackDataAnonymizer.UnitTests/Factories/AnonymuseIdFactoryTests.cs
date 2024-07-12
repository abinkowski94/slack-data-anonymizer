using SlackDataAnonymizer.Factories;

namespace SlackDataAnonymizer.UnitTests.Factories;

public class AnonymousIdFactoryTests
{
    private readonly AnonymousIdFactory sut;

    public AnonymousIdFactoryTests()
    {
        sut = new AnonymousIdFactory();
    }

    [Fact]
    public void GetId_ShouldReturnNonEmptyString()
    {
        // Act
        var result = sut.GetId();

        // Assert
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void GetId_ShouldReturnUniqueString()
    {
        // Act
        var id1 = sut.GetId();
        var id2 = sut.GetId();

        // Assert
        Assert.NotEqual(id1, id2);
    }

    [Fact]
    public void GetId_ShouldReturnGuidInNFormat()
    {
        // Act
        var result = sut.GetId();

        // Assert
        Assert.Equal(32, result.Length);
        Assert.True(Guid.TryParse(result, out _));
    }
}
