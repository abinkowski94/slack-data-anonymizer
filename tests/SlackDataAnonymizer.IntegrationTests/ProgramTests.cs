using SlackDataAnonymizer.IntegrationTests.Fixtures;

namespace SlackDataAnonymizer.IntegrationTests;

public class ProgramTests : IDisposable
{
    private readonly string testSourceDirectory;
    private readonly string testTargetDirectory;
    private readonly string sensitiveDataFile;

    public ProgramTests()
    {
        var currentDir = Directory.GetCurrentDirectory();

        testSourceDirectory = Path.Combine(currentDir, "Data");
        testTargetDirectory = Path.Combine(currentDir, "ProgramTests", "results");
        sensitiveDataFile = Path.Combine(testTargetDirectory, "sensitive-data.json");
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        if (Path.Exists(testTargetDirectory))
        {
            Directory.Delete(testTargetDirectory, true);
        }
    }

    [Fact]
    public async Task RunAsync_WihtYearlyAggregationMode_ShouldProduceSensitiveDataFile()
    {
        // Arrange
        using var app = ApplicationFixture.Create(
            "anonymize",
            "--source-directory",
            testSourceDirectory,
            "--target-directory",
            testTargetDirectory,
            "--aggregation-mode",
            "Yearly",
            "--text-tags",
            "Testers");

        // Act
        await app.RunAsync();

        // Assert
        using var sensitiveData = new FileStream(sensitiveDataFile, FileMode.Open);

        await VerifyJson(sensitiveData);
    }

    [Theory]
    [InlineData(["2019.json"])]
    [InlineData(["2020.json"])]
    [InlineData(["2021.json"])]
    public async Task RunAsync_WihtYearlyAggregationMode_ShouldProduceYearlyFile(string yearlyFile)
    {
        // Arrange
        using var app = ApplicationFixture.Create(
            "anonymize",
            "--source-directory",
            testSourceDirectory,
            "--target-directory",
            testTargetDirectory,
            "--aggregation-mode",
            "Yearly",
            "--text-tags",
            "Testers");

        // Act
        await app.RunAsync();

        // Assert
        using var sensitiveData = new FileStream(Path.Combine(testTargetDirectory, yearlyFile), FileMode.Open);

        var settings = new VerifySettings();
        settings.UseFileName($"{nameof(ProgramTests)}.{nameof(RunAsync_WihtYearlyAggregationMode_ShouldProduceYearlyFile)}_{yearlyFile}");

        await VerifyJson(sensitiveData, settings);
    }
}
