using System;
using System.ComponentModel.DataAnnotations.Schema;
using Hub.Shared.Storage.Repository.Core;
using JetBrains.Annotations;

namespace Spreadsheet.Data.Entities;

[UsedImplicitly]
public class BillingAccountTransaction : EntityBase
{
    [Column]
    [UsedImplicitly]
    public string TransactionId { get; set; }

    [Column]
    [UsedImplicitly]
    public string Key { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [UsedImplicitly]
    public decimal Amount { get; set; }

    [Column]
    [UsedImplicitly]
    public DateTime TransactionDate { get; set; }
}