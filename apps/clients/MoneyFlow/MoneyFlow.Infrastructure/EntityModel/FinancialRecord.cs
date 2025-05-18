using System;
using System.Collections.Generic;

namespace MoneyFlow.Infrastructure.EntityModel;

public partial class FinancialRecord
{
    public int IdFinancialRecord { get; set; }

    public string? RecordName { get; set; }

    public decimal? Ammount { get; set; }

    public string? Description { get; set; }

    public int? IdTransactionType { get; set; }

    public int? IdUser { get; set; }

    public int? IdCategory { get; set; }

    public int? IdSubcategory { get; set; }

    public int? IdAccount { get; set; }

    public DateTime? Date { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }

    public virtual Category? IdCategoryNavigation { get; set; }

    public virtual Subcategory? IdSubcategoryNavigation { get; set; }

    public virtual TransactionType? IdTransactionTypeNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
