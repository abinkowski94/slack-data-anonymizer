using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Abstractions.Repositories;
using System.Text.Json;

namespace SlackDataAnonymizer.Repositories;

public class SensitiveDataWriteRepository(
    string filePath,
    JsonSerializerOptions? options = null) 
    : WriteRepositoryBase(filePath), ISensitiveDataWriteRepository
{
    private readonly JsonSerializerOptions options = options ?? new();

    public async ValueTask CreateSensitiveDataAsync(ISensitiveData sensitiveData, CancellationToken cancellationToken)
    {
        EnsuerDirectoryExists();

        await using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);

        await JsonSerializer.SerializeAsync(fileStream, sensitiveData, options, cancellationToken);
    }
}
