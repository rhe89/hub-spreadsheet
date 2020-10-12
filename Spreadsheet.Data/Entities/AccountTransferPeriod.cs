using System.Collections.Generic;
using Hub.Storage.Entities;

namespace Spreadsheet.Data.Entities
{
    public class AccountTransferPeriod : EntityBase
    {
        public string Period { get; set; }
        public bool CurrentPeriod { get; set; }
        
        public virtual ICollection<AccountTransfer> AccountTransfers { get; set; }
    }
}