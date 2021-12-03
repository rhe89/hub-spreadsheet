using System;
using System.ComponentModel.DataAnnotations.Schema;
using Hub.Shared.Storage.Repository.Core;

namespace Spreadsheet.Data.Dto
{
    public class BillingAccountTransactionDto : EntityDtoBase
    {
        public string TransactionId { get; set; }
        public string Key { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }
    }
}