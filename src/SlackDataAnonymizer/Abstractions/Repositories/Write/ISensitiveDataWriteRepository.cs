using SlackDataAnonymizer.Abstractions.Models;

namespace SlackDataAnonymizer.Abstractions.Repositories.Write;

public interface ISensitiveDataWriteRepository
{
    ValueTask CreateSensitiveDataAsync(ISensitiveData sensitiveData, CancellationToken cancellationToken);
}