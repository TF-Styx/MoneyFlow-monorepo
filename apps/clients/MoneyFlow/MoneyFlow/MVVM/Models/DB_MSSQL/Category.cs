using System;
using System.Collections.Generic;

namespace MoneyFlow.MVVM.Models.DB_MSSQL;

public partial class Category
{
    public int IdCategory { get; set; }

    public string CategoryName { get; set; }

    public string Description { get; set; }

    public string Color { get; set; }

    public byte[] Image { get; set; }

    public int IdUser { get; set; }

    public virtual ICollection<FinancialRecord> FinancialRecords { get; set; } = new List<FinancialRecord>();

    public virtual User IdUserNavigation { get; set; }

    public virtual ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
}
