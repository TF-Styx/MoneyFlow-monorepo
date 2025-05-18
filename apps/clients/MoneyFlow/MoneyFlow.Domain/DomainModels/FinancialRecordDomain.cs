using MoneyFlow.Shared.Constants;

namespace MoneyFlow.Domain.DomainModels
{
    public class FinancialRecordDomain
    {
        private FinancialRecordDomain(int idFinancialRecord, string? recordName, decimal? amount, string? description, int? idTransactionType, int? idUser, int? idCategory, int? idSubcategory, int? idAccount, DateTime? date)
        {
            IdFinancialRecord = idFinancialRecord;
            RecordName = recordName;
            Amount = amount;
            Description = description;
            IdTransactionType = idTransactionType;
            IdUser = idUser;
            IdCategory = idCategory;
            IdSubcategory = idSubcategory;
            IdAccount = idAccount;
            Date = date;
        }

        public int IdFinancialRecord { get; private set; }
        public string? RecordName { get; private set; }
        public decimal? Amount { get; private set; }
        public string? Description { get; private set; }
        public int? IdTransactionType { get; private set; }
        public int? IdUser { get; private set; }
        public int? IdCategory { get; private set; }
        public int? IdSubcategory { get; private set; }
        public int? IdAccount { get; private set; }
        public DateTime? Date { get; private set; }

        public static (FinancialRecordDomain FinancialRecordDomain, string Message) Create(int idFinancialRecord, string? recordName, decimal? amount, string? description, int? idTransactionType, int? idUser, int? idCategory, int? idSubcategory, int? idAccount, DateTime? date)
        {
            var message = string.Empty;

            if (string.IsNullOrWhiteSpace(recordName))
            {
                return (null, "Вы не заполнили поля!!");
            }

            if (recordName.Length > IntConstants.MAX_RECORDNAME_LENGHT &&
                description.Length > IntConstants.MAX_DESCRIPTION_LENGHT &&
                amount > IntConstants.MAX_AMOUNT_LENGHT)
            {
                return (null, "Превышена максимально допустимая длина!!");
            }

            var financialRecord = new FinancialRecordDomain(idFinancialRecord, recordName, amount, description, idTransactionType, idUser, idCategory, idSubcategory, idAccount, date);

            return (financialRecord, message);
        }
    }
}
