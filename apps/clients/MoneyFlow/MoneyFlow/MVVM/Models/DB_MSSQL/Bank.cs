using System;
using System.Collections.Generic;

namespace MoneyFlow.MVVM.Models.DB_MSSQL;

public partial class Bank
{
    public int IdBank { get; set; }

    public string BankName { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
