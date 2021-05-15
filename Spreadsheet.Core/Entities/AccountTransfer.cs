using System.ComponentModel.DataAnnotations.Schema;
using Hub.Storage.Repository.Entities;

namespace Spreadsheet.Core.Entities
{
    public class AccountTransfer : EntityBase
    {
        public long AccountTransferPeriodId { get; set; }
        
        public string AccountName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public Occurence Occurence { get; set; }
        
        public virtual AccountTransferPeriod AccountTransferPeriod { get; set; }
    }

    public enum Occurence
    {
        Once = 0,
        Daily = 1,
        Weekly = 2,
        Monthly = 3,
        Yearly = 4
    }
}