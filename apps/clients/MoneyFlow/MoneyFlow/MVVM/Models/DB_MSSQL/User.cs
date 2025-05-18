using System;
using System.Collections.Generic;

namespace MoneyFlow.MVVM.Models.DB_MSSQL;

public partial class User
{
    public int IdUser { get; set; }

    public string UserName { get; set; }

    public byte[] Avatar { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public int? IdGender { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<FinancialRecord> FinancialRecords { get; set; } = new List<FinancialRecord>();

    public virtual Gender IdGenderNavigation { get; set; }
}
