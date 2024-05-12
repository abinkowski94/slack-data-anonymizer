using SlackDataAnonymizer.Abstractions.Models;

namespace SlackDataAnonymizer.Abstractions.Repositories;

public interface ISensitiveDataWriteRepository
{
    ValueTask CreateSensitiveDataAsync(ISensitiveData sensitiveData, CancellationToken cancellationToken);
}