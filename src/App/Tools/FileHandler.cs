namespace SerialCaller.App.Tools;

/// <summary>
/// File handler class.
/// </summary>
public static class FileHandler
{
    private static readonly string FOLDER_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"Documents\SerialCaller");

    /// <summary>
    /// Will check if the folder exists and create it if it doesn't.
    /// </summary>
    public static void CheckFolder(string? subFolderPath = null)
    {
        subFolderPath ??= string.Empty;
        string path = Path.Combine(FOLDER_PATH, subFolderPath);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    /// <summary>
    /// Will get the filePath and create the file with default content if it doesn't exist.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="defaultContent"></param>
    /// <returns></returns>
    public static string GetFilePath(string fileName)
    {
        var filePath = Path.Combine(FOLDER_PATH, fileName);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"{fileName} in {FOLDER_PATH}, please created it there first.");
        }

        return filePath;
    }
}
