using System;
using System.Collections.Generic;

namespace MoneyFlow.Infrastructure.EntityModel;

public partial class Account
{
    public int IdAccount { get; set; }

    public int? NumberAccount { get; set; }

    public int? IdUser { get; set; }

    public int? IdBank { get; set; }

    public int? IdAccountType { get; set; }

    public decimal? Balance { get; set; }

    public virtual ICollection<FinancialRecord> FinancialRecords { get; set; } = new List<FinancialRecord>();

    public virtual AccountType? IdAccountTypeNavigation { get; set; }

    public virtual Bank? IdBankNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
