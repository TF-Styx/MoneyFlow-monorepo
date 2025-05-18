using System;
using System.Collections.Generic;

namespace MoneyFlow.MVVM.Models.DB_MSSQL;

public partial class AccountType
{
    public int IdAccountType { get; set; }

    public string AccountTypeName { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
