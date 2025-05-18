using System;
using System.Collections.Generic;

namespace MoneyFlow.MVVM.Models.DB_MSSQL;

public partial class TransactionType
{
    public int IdTransactionType { get; set; }

    public string TransactionTypeName { get; set; }

    public string Description { get; set; }

    public virtual ICollection<FinancialRecord> FinancialRecords { get; set; } = new List<FinancialRecord>();
}
