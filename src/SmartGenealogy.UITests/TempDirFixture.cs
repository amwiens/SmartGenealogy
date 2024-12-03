namespace SmartGenealogy.UITests;

public class TempDirFixture : IDisposable
{
    public static string ModuleTempDir { get; set; }

    static TempDirFixture()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "SmartGenealogyTest");

        if (Directory.Exists(tempDir))
        {
            Directory.Delete(tempDir, true);
        }
        Directory.CreateDirectory(tempDir);

        ModuleTempDir = tempDir;

        Console.WriteLine($"Using temp dir: {ModuleTempDir}");
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (Directory.Exists(ModuleTempDir))
        {
            Console.WriteLine($"Deleting temp dir: {ModuleTempDir}");
            Directory.Delete(ModuleTempDir, true);
        }

        GC.SuppressFinalize(this);
    }
}

[CollectionDefinition("TempDir")]
public class TempDirCollection : ICollectionFixture<TempDirFixture> { }