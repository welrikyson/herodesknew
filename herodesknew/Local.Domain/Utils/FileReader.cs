using System.Text;

namespace herodesknew.Local.Domain.Utils;

public class FileReader
{
    public static string? ReadFirstLineFromFile(string filePath)
    {
        using StreamReader streamReader = new(filePath);        
        return streamReader.ReadLine(); ;
    }

    public static string? ReadFileContent(string filePath)
    {       
        using StreamReader reader = new(filePath, Encoding.Latin1);
        return reader.ReadToEnd();
    }
}
