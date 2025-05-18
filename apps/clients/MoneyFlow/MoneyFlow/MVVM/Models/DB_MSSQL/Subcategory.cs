using System;
using System.Collections.Generic;

namespace MoneyFlow.MVVM.Models.DB_MSSQL;

public partial class Subcategory
{
    public int IdSubcategory { get; set; }

    public string SubcategoryName { get; set; }

    public string Description { get; set; }

    public byte[] Image { get; set; }

    public int IdCategory { get; set; }

    public virtual ICollection<FinancialRecord> FinancialRecords { get; set; } = new List<FinancialRecord>();

    public virtual Category IdCategoryNavigation { get; set; }
}
