namespace SlackDataAnonymizer.Exceptions;

public class FileReadingException(string file, Exception? innerException) 
    : Exception($"Failed to read file \"{file}\".", innerException)
{
}
