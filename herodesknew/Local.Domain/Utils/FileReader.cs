using System.Text;

namespace herodesknew.Local.Domain.Utils;

public class FileReader
{
    public static string? ReadFirstLineFromFile(string filePath)
    {
        using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read);
        using StreamReader streamReader = new(fileStream);

        // Lê a primeira linha do arquivo
        string? firstLine = streamReader.ReadLine();

        return firstLine;
    }
    public static string? ReadFileContent(string filePath)
    {
        using (StreamReader reader = new StreamReader(filePath, Encoding.Latin1))
        {
            return reader.ReadToEnd();
        }
    }
}
