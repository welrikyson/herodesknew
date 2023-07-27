using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace herodesknew.Local.Domain.Utils;

public class SpreadsheetHelper : ISpreadsheetHelper
{
    public bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }

    public string? GetCellValue(string filePath, string cellReference)
    {
        using var document = SpreadsheetDocument.Open(filePath, true);
        var worksheetPart = document.WorkbookPart?.WorksheetParts.Last();
        var cells = worksheetPart?.Worksheet.Descendants<Cell>();
        var cell = cells?.FirstOrDefault(c => c.CellReference == cellReference);
        return cell?.CellValue?.Text;
    }

    public void UpdateCellValue(IEnumerable<Cell> cells, string cellReference, string cellValue)
    {
        var cell = cells.FirstOrDefault(c => c.CellReference == cellReference);
        if (cell != null)
        {
            cell.CellValue = new CellValue(cellValue);
            cell.DataType = new EnumValue<CellValues>(CellValues.String);
        }
    }
}
