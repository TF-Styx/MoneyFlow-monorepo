namespace MoneyFlow.Domain.DomainModels
{
    public class UserAccountTypesDomain
    {
        private UserAccountTypesDomain(int idUser, IEnumerable<AccountTypeDomain> accountTypes)
        {
            IdUser = idUser;

            foreach (var item in accountTypes)
            {
                AccountTypes.Add(item);
            }
        }

        public int IdUser {  get; private set; }
        public List<AccountTypeDomain> AccountTypes { get; private set; } = [];

        public static (UserAccountTypesDomain UserAccountTypesDomain, string Message) Create(int idUser, List<AccountTypeDomain> accountTypes)
        {
            var message = string.Empty;
            var userAccountTypes = new UserAccountTypesDomain(idUser, accountTypes);

            return (userAccountTypes, message);
        }

        public void AddNewType(AccountTypeDomain accountType)
        {
            if (string.IsNullOrWhiteSpace(accountType.AccountTypeName)) { throw new Exception("Наименование типа счета отсутствует!!"); }

            AccountTypes.Add(accountType);
        }
    }
}
