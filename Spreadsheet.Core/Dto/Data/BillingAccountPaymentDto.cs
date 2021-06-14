using System;
using System.ComponentModel.DataAnnotations.Schema;
using Hub.Storage.Repository.Dto;

namespace Spreadsheet.Core.Dto.Data
{
    public class BillingAccountPaymentDto : EntityDtoBase
    {
        public string TransactionId { get; set; }
        public string Key { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }
    }
}