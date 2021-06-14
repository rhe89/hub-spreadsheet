using System;
using Spreadsheet.Core.Dto.Spreadsheet;
using Spreadsheet.Core.Extensions;

namespace Spreadsheet.Core.Dto.Integration
{
    public class TransactionDto : Cell
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public long AccountId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int TransactionType { get; set; }
        public string TransactionId { get; set; }

        public override string RowKey { get; set; }
        public override string CellValue => Amount.ToComma();
    }
}