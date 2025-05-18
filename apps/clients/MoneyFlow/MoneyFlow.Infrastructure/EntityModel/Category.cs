using System;
using System.Collections.Generic;

namespace MoneyFlow.Infrastructure.EntityModel;

public partial class Category
{
    public int IdCategory { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Color { get; set; }

    public byte[]? Image { get; set; }

    public int IdUser { get; set; }

    public virtual ICollection<CatLinkSub> CatLinkSubs { get; set; } = new List<CatLinkSub>();

    public virtual ICollection<FinancialRecord> FinancialRecords { get; set; } = new List<FinancialRecord>();

    public virtual User IdUserNavigation { get; set; } = null!;
}
