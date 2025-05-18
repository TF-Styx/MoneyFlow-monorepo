namespace MoneyFlow.Domain.DomainModels
{
    public class FinancialRecordViewingDomain
    {
        private FinancialRecordViewingDomain
            (
                int idFinancialRecord, 
                string? recordName, 
                decimal? amount, 
                string? description, 
                int? idTransactionType,
                string transactionTypeName, 
                int? idUser, 
                int? idCategory, 
                string? categoryName, 
                int? idSubcategory, 
                string? subcategoryName, 
                int? accountNumber, 
                DateTime? date
            )
        {
            IdFinancialRecord = idFinancialRecord;
            RecordName = recordName;
            Amount = amount;
            Description = description;
            IdTransactionType = idTransactionType;
            TransactionTypeName = transactionTypeName;
            IdUser = idUser;
            IdCategory = idCategory;
            CategoryName = categoryName == null ? "Отсутствует!" : categoryName;
            IdSubcategory = idSubcategory;
            SubcategoryName = subcategoryName == null ? "Отсутствует!" : subcategoryName;
            AccountNumber = accountNumber;
            Date = date;
        }

        public int IdFinancialRecord { get; private set; }
        public string? RecordName { get; private set; }
        public decimal? Amount { get; private set; }
        public string? Description { get; private set; }
        public int? IdTransactionType { get; private set; }
        public string TransactionTypeName { get; private set; }
        public int? IdUser { get; private set; }
        public int? IdCategory { get; set; }
        public string? CategoryName { get; private set; }
        public int? IdSubcategory { get; set; }
        public string? SubcategoryName { get; private set; }
        public int? AccountNumber { get; private set; }
        public DateTime? Date { get; private set; }

        public static (FinancialRecordViewingDomain FinancialRecordViewingDomain, string Message) Create
            (
                int idFinancialRecord, 
                string? recordName, 
                decimal? amount, 
                string? description, 
                int? idTransactionType,
                string transactionTypeName, 
                int? idUser, 
                int? idCategory, 
                string? categoryName, 
                int? idSubcategory, 
                string? subcategoryName, 
                int? accountNumber, 
                DateTime? date
            )
        {
            var message = string.Empty;

            var financialRecord = new FinancialRecordViewingDomain
                (
                    idFinancialRecord, 
                    recordName, 
                    amount, 
                    description, 
                    idTransactionType, 
                    transactionTypeName, 
                    idUser, 
                    idCategory,
                    categoryName,
                    idSubcategory,
                    subcategoryName, 
                    accountNumber, 
                    date);

            return (financialRecord, message);
        }
    }
}
