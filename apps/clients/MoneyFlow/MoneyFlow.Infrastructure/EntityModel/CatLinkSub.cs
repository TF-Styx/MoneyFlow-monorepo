using System;
using System.Collections.Generic;

namespace MoneyFlow.Infrastructure.EntityModel;

public partial class CatLinkSub
{
    public int IdCategory { get; set; }

    public int IdSubcategory { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int IdUser { get; set; }

    public virtual Category IdCategoryNavigation { get; set; } = null!;

    public virtual Subcategory IdSubcategoryNavigation { get; set; } = null!;
}
