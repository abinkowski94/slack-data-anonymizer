using SlackDataAnonymizer.Abstractions.Repositories.Write;

namespace SlackDataAnonymizer.Abstractions.Factories;
public interface ISensitiveDataWriteRepositoryFactory
{
    ISensitiveDataWriteRepository Create(string filePath);
}