using SlackDataAnonymizer.Abstractions.Models;

namespace SlackDataAnonymizer.Abstractions.Service;

public interface IAnonymizerService<T>
{
    public IEnumerable<T?>? Anonymize(IEnumerable<T?>? values, ISensitiveData sensitiveData)
    {
        if (values is null)
        {
            return values;
        }

        foreach (var value in values)
        {
            Anonymize(value, sensitiveData);
        }

        return values;
    }

    T? Anonymize(T? value, ISensitiveData sensitiveData);
}