using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using herodesknew.Domain.Utils;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace herodesknew.Local.Application;


public class FileOpener
{
    public static void Open(string fileName)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = fileName,
            UseShellExecute = true
        });
    }
}

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

public class FolderSearcher
{
    public static string FindFolderInDirectory(string targetFolderName, string rootDirectory)
    {
        Queue<string> folderQueue = new Queue<string>();
        folderQueue.Enqueue(rootDirectory);

        while (folderQueue.Count > 0)
        {
            string currentFolder = folderQueue.Dequeue();
            string[] subFolders = Directory.GetDirectories(currentFolder);

            foreach (string subFolder in subFolders)
            {
                string folderName = Path.GetFileName(subFolder);

                if (folderName == targetFolderName)
                {
                    return subFolder; // Pasta encontrada!                        
                }

                folderQueue.Enqueue(subFolder);
            }
        }

        return null; // Pasta não encontrada
    }
}

public class SqlExecutionPlanDoc
{
    public static readonly string RefCellPullRequestUrl = "D44";
    public static readonly string refCellMotivo = "D11";
    public static readonly string _fileName = "SIST 030 - 01 - Plano de Deploy.xlsx";

    private readonly ISpreadsheetHelper _speadsheetHelper;
    private readonly string _planoDeployfilePath;
    private readonly IUrlParser _urlParser;

    public SqlExecutionPlanDoc(ISpreadsheetHelper speadsheetHelper, IUrlParser urlParser)
    {
        _planoDeployfilePath = Path.Combine(Environment.CurrentDirectory, _fileName);
        _speadsheetHelper = speadsheetHelper;
        _urlParser = urlParser;
    }

    public int? GetPullRequestId(string path)
    {
        var fileFullName = Path.Combine(path, _fileName);

        if (!File.Exists(fileFullName))
        {
            return null;

        }
        var pullRequestUrl = _speadsheetHelper.GetCellValue(fileFullName, RefCellPullRequestUrl);

        if (string.IsNullOrEmpty(pullRequestUrl))
        {
            return null;
        }

        if (!_urlParser.TryGetPullRequestNumberFromUrl(pullRequestUrl, out var pullRequestId))
        {
            return null;
        }

        return pullRequestId;
    }
    public void CreateDeployDocAsync(string pastaDestino, string title, string pullRequestUrl)
    {
        string caminhoDestino = Path.Combine(pastaDestino, _fileName);

        if (File.Exists(caminhoDestino))
        {
            File.Delete(caminhoDestino);
            Console.WriteLine("Arquivo existente removido.");
        }

        File.Copy(_planoDeployfilePath, caminhoDestino);

        FillPlanoDeploySheet(pullRequestUrl, title, caminhoDestino);
    }

    public void FillPlanoDeploySheet(string valueCellPullRequestUrl, string valueCellmotivo, string caminhoDestino)
    {
        using var document = SpreadsheetDocument.Open(caminhoDestino, true);

        var worksheetPart = document.WorkbookPart?.WorksheetParts.Last();
        var cells = worksheetPart?.Worksheet.Descendants<Cell>();

        _speadsheetHelper.UpdateCellValue(cells, RefCellPullRequestUrl, valueCellPullRequestUrl);
        _speadsheetHelper.UpdateCellValue(cells, refCellMotivo, valueCellmotivo);

        worksheetPart?.Worksheet.Save(); // save changes to the worksheet      
    }
}

