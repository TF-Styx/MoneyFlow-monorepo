using MoneyFlow.Shared.Constants;

namespace MoneyFlow.Domain.DomainModels
{
    public class TransactionTypeDomain
    {
        private TransactionTypeDomain(int idTransactionType, string? transactionTypeName, string? description)
        {
            IdTransactionType = idTransactionType;
            TransactionTypeName = transactionTypeName;
            Description = description;
        }

        public int IdTransactionType { get; private set; }
        public string? TransactionTypeName { get; private set; }
        public string? Description { get; private set; }

        public static (TransactionTypeDomain TransactionTypeDomain, string Message) Create(int idTransactionType, string? transactionTypeName, string? description)
        {
            string message = string.Empty;

            if (string.IsNullOrWhiteSpace(transactionTypeName)) { return (null, "Вы не заполнили поля!!"); }

            if (transactionTypeName.Length > IntConstants.MAX_TRANSACTIONTYPENAME_LENGHT &&
                description.Length > IntConstants.MAX_DESCRIPTION_LENGHT)
            {
                return (null, "Первышена допустимая длина!!");
            }

            var domain = new TransactionTypeDomain(idTransactionType, transactionTypeName, description);

            return (domain, message);
        }
    }
}
