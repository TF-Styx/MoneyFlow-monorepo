using MoneyFlow.MVVM.Models.DB_MSSQL;

namespace MoneyFlow.MVVM.Models.AppModel
{
    public class FinancialRecordDTO
    {
        public int IdFinancialRecord { get; set; }

        public string RecordName { get; set; }

        public decimal? Ammount { get; set; }

        public string Description { get; set; }

        public TransactionType TransactionType { get; set; }

        public User User { get; set; }

        public Category Category { get; set; }

        public Account Account { get; set; }

        public Bank Bank { get; set; }

        public DateTime? Date { get; set; }

        public Subcategory Subcategory { get; set; }
    }
}
