using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using herodesknew.Domain.Utils;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace herodesknew.Application.Local;


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
        using FileStream fileStream = new (filePath, FileMode.Open, FileAccess.Read);
        using StreamReader streamReader = new (fileStream);

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

    public SqlExecutionPlanDoc(ISpreadsheetHelper speadsheetHelper)
    {
        _planoDeployfilePath = Path.Combine(Environment.CurrentDirectory, _fileName);
        _speadsheetHelper = speadsheetHelper;
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

public class TicketSqlFolderManager
{
    private readonly string _rootTicketFolderPath;

    public TicketSqlFolderManager(string rootTicketFolderPath)
    {
        _rootTicketFolderPath = rootTicketFolderPath;
    }

    public string GetOrCreateTicketSqlFolder(int ticketId)
    {
        string ticketSqlFolder = FolderSearcher.FindFolderInDirectory(_rootTicketFolderPath, $"{ticketId}");
        //TODO: validar se a folder tem o formato /../../ANO/MES/TICKETID/SLT
        if(ticketSqlFolder != null)
        {
            return ticketSqlFolder;
        }
            
        var dirInfo = Directory.CreateDirectory(Path.Combine(_rootTicketFolderPath, @$"{DateTime.Now:yyyy\\MM}", ticketId.ToString(), "SLT"));
        return dirInfo.FullName;
    }
}

