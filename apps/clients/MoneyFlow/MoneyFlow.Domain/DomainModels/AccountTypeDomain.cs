using MoneyFlow.Shared.Constants;

namespace MoneyFlow.Domain.DomainModels
{
    public class AccountTypeDomain
    {
        private AccountTypeDomain(int idAccountType, string? accountTypeName)
        {
            IdAccountType = idAccountType;
            AccountTypeName = accountTypeName;
        }

        public int IdAccountType { get; private set; }
        public string? AccountTypeName { get; private set; }

        public static (AccountTypeDomain AccountTypeDomain, string Message) Create(int idAccountType, string? accountTypeName)
        {
            var message = string.Empty;

            if (string.IsNullOrWhiteSpace(accountTypeName))
            {
                return (null, "Вы не заполнили поля!!");
            }

            if (accountTypeName.Length > IntConstants.MAX_ACCOUNT_TYPE_NAME_LENGHT)
            {
                return (null, "Превышена допустимая длина в «255» символов");
            }

            var accountType = new AccountTypeDomain(idAccountType, accountTypeName);

            return (accountType, message);
        }
    }
}
