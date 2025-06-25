namespace SerialCaller.Libs.Common.Logger;

public static class FileLogger
{
    public static void Log(string filePath, string text)
    {
        var folderName = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(folderName)) 
        {
            Directory.CreateDirectory(folderName!);
        }

        File.WriteAllText(filePath, text);
    }
}
