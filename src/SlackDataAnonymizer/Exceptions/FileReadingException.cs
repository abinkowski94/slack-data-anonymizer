namespace SlackDataAnonymizer.Exceptions;

public class FileReadingException(string file, Exception? innerException = null)
    : Exception($"Failed to read file \"{file}\".", innerException)
{
    public string FilePath { get; } = file;
}
