using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace herodesknew.Local.Domain.Utils;

public class SqlExecutionPlanDoc
{
    public static readonly string RefCellPullRequestUrl = "D44";
    public static readonly string refCellMotivo = "D11";
    public static readonly string FileName = "SIST 030 - 01 - Plano de Deploy.xlsx";

    private readonly ISpreadsheetHelper _speadsheetHelper;
    private readonly string _planoDeployfilePath;
    private readonly IUrlParser _urlParser;

    public SqlExecutionPlanDoc(ISpreadsheetHelper speadsheetHelper, IUrlParser urlParser)
    {
        _planoDeployfilePath = Path.Combine(Environment.CurrentDirectory, FileName);
        _speadsheetHelper = speadsheetHelper;
        _urlParser = urlParser;
    }

    public int? GetPullRequestId(string path)
    {
        var fileFullName = Path.Combine(path, FileName);

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
        string caminhoDestino = Path.Combine(pastaDestino, FileName);

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
