using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;

namespace herodesknew.Local.Domain.Utils;

public static class TicketFolderManager
{
    public static List<IEnumerable<(int Id, string dirSltFullName, string PathFullName, int TicketId)>> GetTicketSubDirectoriesWithScripts(int[] tickets)
    {
        var ticketDir = new DirectoryInfo(TicketFolderSettings.Root);
        var currentYear = DateTime.Now.Year;
        var dx = ticketDir.GetDirectories();
        var yearDirectories = ticketDir.GetDirectories().Where(dir => dir.Name.IsNumber((v) => v <= currentYear));
        var directories = yearDirectories.SelectMany(d => d.GetDirectories().Where(dir => dir.Name.IsNumber((v) => v >= 1 && v <= 12)));

        var validTicketDirectories = directories
            .SelectMany(d => d.GetDirectories().Where(d => d.Name.IsNumber()))
            .Where(dir => dir.Name.IsNumber())
            .Where(d => tickets.Length == 0 || tickets.Contains(Convert.ToInt32(d.Name)))
            .Select(dir => new
            {
                TicketId = Convert.ToInt32(dir.Name),
                Dir = dir
            });

        var ticketScriptFolders = validTicketDirectories.Select(ticketDir => new
        {
            ticketDir.TicketId,
            Dir = new DirectoryInfo(Path.Combine(ticketDir.Dir.FullName, TicketFolderSettings.ScriptFolderName))
        });

        var result = (
            from ticketScriptFolder in ticketScriptFolders
            let dirSlt = ticketScriptFolder.Dir
            let someItem = ticketScriptFolder.Dir.GetDirectories()
            .Where(dir => dir.Name.IsNumber())
            .Select(dir =>
            {
                return (Id: Convert.ToInt32(dir.Name), dirSltFullName: dirSlt.FullName, PathFullName: dir.FullName, ticketScriptFolder.TicketId);
            })
            select someItem).ToList();

        return result;
    }
}

public static class SqlExecutionPlanDoc
{
    public static readonly string RefCellPullRequestUrl = "D44";
    public static readonly string refCellMotivo = "D11";
    public static readonly string FileName = "SIST 030 - 01 - Plano de Deploy.xlsx";
    private static readonly string _planoDeployfilePath;
    public static readonly string PathDefaultToUse;
    static SqlExecutionPlanDoc()
    {
        _planoDeployfilePath = Path.Combine(Environment.CurrentDirectory, FileName);
        PathDefaultToUse = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    }

    public static int? GetPullRequestId(string path)
    {
        var fileFullName = Path.Combine(path, FileName);

        if (!File.Exists(fileFullName))
        {
            return null;

        }

        var pullRequestUrl = SpreadsheetHelper.GetCellValue(fileFullName, RefCellPullRequestUrl);

        if (string.IsNullOrEmpty(pullRequestUrl))
        {
            return null;
        }

        if (!UrlParser.TryGetPullRequestNumberFromUrl(pullRequestUrl, out var pullRequestId))
        {
            return null;
        }

        return pullRequestId;
    }

    public static void CreateDeployDocAsync(string pastaDestino, string title, string pullRequestUrl)
    {
        string caminhoDestino = Path.Combine(pastaDestino, FileName);

        if (File.Exists(caminhoDestino))
        {
            File.Delete(caminhoDestino);
            Console.WriteLine("Arquivo existente removido.");
        }

        File.Copy(_planoDeployfilePath, caminhoDestino);

        FillPlanoDeploySheet(pullRequestUrl, title, caminhoDestino);
    }

    private static void FillPlanoDeploySheet(string valueCellPullRequestUrl, string valueCellmotivo, string caminhoDestino)
    {
        using var document = SpreadsheetDocument.Open(caminhoDestino, true);

        var worksheetPart = document.WorkbookPart?.WorksheetParts.Last();
        var cells = worksheetPart?.Worksheet.Descendants<Cell>();

        SpreadsheetHelper.UpdateCellValue(cells, RefCellPullRequestUrl, valueCellPullRequestUrl);
        SpreadsheetHelper.UpdateCellValue(cells, refCellMotivo, valueCellmotivo);

        worksheetPart?.Worksheet.Save(); // save changes to the worksheet      
    }
}

public static class StringExtension
{
    public static bool IsNumber(this string text, Func<int, bool>? validate = null)
    {
        if (int.TryParse(text, out var value))
        {
            if (validate?.Invoke(value) ?? true)
            {
                return true;
            }
        }
        return false;
    }
}

public static class UrlParser
{
    public static bool TryGetPullRequestNumberFromUrl(string url, out int pullRequestNumber)
    {
        var match = Regex.Match(url, @"/pullrequest/(\d+)");

        if (!match.Success || !int.TryParse(match.Groups[1].Value, out pullRequestNumber))
        {
            pullRequestNumber = 0;
            return false;
        }

        return true;
    }
}

public static class SpreadsheetHelper
{
    public static string? GetCellValue(string filePath, string cellReference)
    {
        using var document = SpreadsheetDocument.Open(filePath, true);
        var worksheetPart = document.WorkbookPart?.WorksheetParts.Last();
        var cells = worksheetPart?.Worksheet.Descendants<Cell>();
        var cell = cells?.FirstOrDefault(c => c.CellReference == cellReference);
        return cell?.CellValue?.Text;
    }

    public static void UpdateCellValue(IEnumerable<Cell> cells, string cellReference, string cellValue)
    {
        var cell = cells.FirstOrDefault(c => c.CellReference == cellReference);
        if (cell != null)
        {
            cell.CellValue = new CellValue(cellValue);
            cell.DataType = new EnumValue<CellValues>(CellValues.String);
        }
    }
}

public static class TicketFolderSettings
{
    public static readonly string Root;
    public static readonly string ScriptFolderName = "SLT";
    static TicketFolderSettings()
    {
        Root = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MS", "src", "HD");
    }
}
