using DocumentFormat.OpenXml.Spreadsheet;

namespace herodesknew.Local.Domain.Utils
{
    public interface ISpreadsheetHelper
    {
        bool FileExists(string filePath);
        string? GetCellValue(string filePath, string cellReference);
        void UpdateCellValue(IEnumerable<Cell> cells, string cellReference, string cellValue);
    }
}
