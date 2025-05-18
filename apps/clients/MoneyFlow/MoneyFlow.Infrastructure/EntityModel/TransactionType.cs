using System;
using System.Collections.Generic;

namespace MoneyFlow.Infrastructure.EntityModel;

public partial class TransactionType
{
    public int IdTransactionType { get; set; }

    public string TransactionTypeName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<FinancialRecord> FinancialRecords { get; set; } = new List<FinancialRecord>();
}
