using System;
using System.Collections.Generic;

namespace MoneyFlow.MVVM.Models.DB_MSSQL;

public partial class Gender
{
    public int IdGender { get; set; }

    public string GenderName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
