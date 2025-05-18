using System;
using System.Collections.Generic;

namespace MoneyFlow.Infrastructure.EntityModel;

public partial class Subcategory
{
    public int IdSubcategory { get; set; }

    public string SubcategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public byte[]? Image { get; set; }

    public int IdUser { get; set; }

    public virtual ICollection<CatLinkSub> CatLinkSubs { get; set; } = new List<CatLinkSub>();

    public virtual ICollection<FinancialRecord> FinancialRecords { get; set; } = new List<FinancialRecord>();
}
