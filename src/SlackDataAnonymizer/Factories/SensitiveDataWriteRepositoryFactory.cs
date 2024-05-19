using SlackDataAnonymizer.Abstractions.Factories;
using SlackDataAnonymizer.Abstractions.Repositories.Write;
using SlackDataAnonymizer.Repositories.Write;
using System.Text.Json;

namespace SlackDataAnonymizer.Factories;

public class SensitiveDataWriteRepositoryFactory(JsonSerializerOptions serializerOptions) : ISensitiveDataWriteRepositoryFactory
{
    private readonly JsonSerializerOptions serializerOptions = serializerOptions;

    public ISensitiveDataWriteRepository Create(string filePath)
    {
        return new SensitiveDataWriteRepository(filePath, serializerOptions);
    }
}
