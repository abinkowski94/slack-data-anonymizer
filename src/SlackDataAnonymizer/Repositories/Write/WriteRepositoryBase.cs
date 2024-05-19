namespace SlackDataAnonymizer.Repositories.Write;

public class WriteRepositoryBase(string filePath)
{
    protected readonly string filePath = filePath;

    protected void EnsuerDirectoryExists()
    {
        var directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath) && !string.IsNullOrWhiteSpace(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
}