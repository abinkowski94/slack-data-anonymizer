namespace SlackDataAnonymizer.Repositories.Write;

public abstract class WriteRepositoryBase
{
    protected static void EnsuerDirectoryExists(string path)
    {
        var directoryPath = Path.GetDirectoryName(path);

        if (!Directory.Exists(directoryPath) && !string.IsNullOrWhiteSpace(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
}