using System;
using System.Collections.Generic;

namespace MoneyFlow.Infrastructure.EntityModel;

public partial class AccountType
{
    public int IdAccountType { get; set; }

    public string AccountTypeName { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
