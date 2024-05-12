using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Repositories;
using System.Text.Json;

namespace SlackDataAnonymizer.Repositories;

public class SensitiveDataWriteRepository(
    string filePath,
    JsonSerializerOptions? options = null) : ISensitiveDataWriteRepository
{
    private readonly string messagesFilePath = filePath;
    private readonly JsonSerializerOptions options = options ?? new();

    public async ValueTask CreateSensitiveDataAsync(ISensitiveData sensitiveData, CancellationToken cancellationToken)
    {
        await using var fileStream = new FileStream(messagesFilePath, FileMode.OpenOrCreate, FileAccess.Write);

        await JsonSerializer.SerializeAsync(fileStream, sensitiveData, options, cancellationToken);
    }
}
