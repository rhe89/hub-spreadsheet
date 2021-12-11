namespace Spreadsheet.Integration.Dto.Spreadsheet
{
    public interface ICell
    {
        string RowKey { get; }
        string CellValue { get; }
    }
}