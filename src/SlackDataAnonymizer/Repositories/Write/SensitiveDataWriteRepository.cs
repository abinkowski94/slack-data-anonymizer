using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Repositories.Write;
using System.Text.Json;

namespace SlackDataAnonymizer.Repositories.Write;

public class SensitiveDataWriteRepository(
    string filePath,
    JsonSerializerOptions? options = null)
    : WriteRepositoryBase, ISensitiveDataWriteRepository
{
    private readonly string filePath = filePath;
    private readonly JsonSerializerOptions options = options ?? new();

    public async ValueTask CreateSensitiveDataAsync(ISensitiveData sensitiveData, CancellationToken cancellationToken)
    {
        EnsuerDirectoryExists(filePath);

        await using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);

        await JsonSerializer.SerializeAsync(fileStream, sensitiveData, options, cancellationToken);
    }
}
