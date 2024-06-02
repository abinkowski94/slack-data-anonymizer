using SlackDataAnonymizer.Abstractions.Models;
using SlackDataAnonymizer.Commands;

namespace SlackDataAnonymizer.Abstractions.Service;

public interface IAnonymizerService<T>
{
    public IEnumerable<T?>? Anonymize(IEnumerable<T?>? values, AnonymizeDataCommand command, ISensitiveData sensitiveData)
    {
        if (values is null)
        {
            return values;
        }

        foreach (var value in values)
        {
            Anonymize(value, command, sensitiveData);
        }

        return values;
    }

    T? Anonymize(T? value, AnonymizeDataCommand command, ISensitiveData sensitiveData);
}