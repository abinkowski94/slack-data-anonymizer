using SlackDataAnonymizer.Models.Maps;
using SlackDataAnonymizer.Models.Slack;

namespace SlackDataAnonymizer.UnitTests.Models.Maps;

public class SensitiveDataTests
{
    [Fact]
    public void Constructor_ShouldInitializeEmptyCollections()
    {
        // Arrange & Act
        var sensitiveData = new SensitiveData();

        // Assert
        Assert.Empty(sensitiveData.UserIds);
        Assert.Empty(sensitiveData.UserDetails);
        Assert.Empty(sensitiveData.TextTags);
    }

    [Fact]
    public void Constructor_ShouldInitializeWithGivenCollections()
    {
        // Arrange
        var userIds = new Dictionary<string, string> { { "user1", "anonymizedUser1" } };
        var userDetails = new Dictionary<string, UserDetails> { { "anonymizedUser1", new UserDetails { AnonymizedId = "anonymizedUser1", UserId = "user1" } } };
        var textTags = new Dictionary<string, string> { { "tag1", "anonymizedTag1" } };

        // Act
        var sensitiveData = new SensitiveData(userIds, userDetails, textTags);

        // Assert
        Assert.Equal("anonymizedUser1", sensitiveData.UserIds["user1"]);
        Assert.Equal("anonymizedUser1", sensitiveData.UserDetails["anonymizedUser1"].AnonymizedId);
        Assert.Equal("anonymizedTag1", sensitiveData.TextTags["tag1"]);
    }

    [Fact]
    public void GetOrAddUser_ShouldReturnExistingAnonymizedId()
    {
        // Arrange
        var userIds = new Dictionary<string, string> { { "user1", "anonymizedUser1" } };
        var sensitiveData = new SensitiveData(userIds, new Dictionary<string, UserDetails>(), new Dictionary<string, string>());

        // Act
        var result = sensitiveData.GetOrAddUser("user1");

        // Assert
        Assert.Equal("anonymizedUser1", result);
    }

    [Fact]
    public void GetOrAddUser_ShouldAddNewUserAndReturnAnonymizedId()
    {
        // Arrange
        var sensitiveData = new SensitiveData();

        // Act
        var result = sensitiveData.GetOrAddUser("user1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result, sensitiveData.UserIds["user1"]);
        Assert.Equal("user1", sensitiveData.UserDetails[result].UserId);
    }

    [Fact]
    public void UpdateUserProfileWithId_ShouldUpdateUserProfile()
    {
        // Arrange
        var sensitiveData = new SensitiveData();
        var userProfile = new UserProfile { RealName = "John Doe" };
        var anonymizedId = sensitiveData.GetOrAddUser("user1", userProfile);

        // Act
        var newProfile = new UserProfile { RealName = "Jane Doe" };
        sensitiveData.UpdateUserProfileWithId("user1", newProfile);

        // Assert
        Assert.Equal("Jane Doe", sensitiveData.UserDetails[anonymizedId].Profile?.RealName);
    }

    [Fact]
    public void GetOrAddTag_ShouldReturnExistingAnonymizedTag()
    {
        // Arrange
        var textTags = new Dictionary<string, string> { { "tag1", "anonymizedTag1" } };
        var sensitiveData = new SensitiveData(new Dictionary<string, string>(), new Dictionary<string, UserDetails>(), textTags);

        // Act
        var result = sensitiveData.GetOrAddTag("tag1");

        // Assert
        Assert.Equal("anonymizedTag1", result);
    }

    [Fact]
    public void GetOrAddTag_ShouldAddNewTagAndReturnAnonymizedTag()
    {
        // Arrange
        var sensitiveData = new SensitiveData();

        // Act
        var result = sensitiveData.GetOrAddTag("tag1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result, sensitiveData.TextTags["tag1"]);
    }
}
