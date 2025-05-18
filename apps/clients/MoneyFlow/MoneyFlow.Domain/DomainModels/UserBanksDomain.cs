namespace MoneyFlow.Domain.DomainModels
{
    public class UserBanksDomain
    {
        private UserBanksDomain(int idUser, IEnumerable<BankDomain> banks)
        {
            IdUser = idUser;

            foreach (var item in banks)
            {
                Banks.Add(item);
            }
        }

        public int IdUser { get; set; } // private set;
        public List<BankDomain> Banks { get; set; } = new List<BankDomain>(); // private set;

        public void AddNewBank(BankDomain bank)
        {
            if (string.IsNullOrWhiteSpace(bank.BankName)) { throw new Exception("Наименование банка отсутствует!!"); }

            Banks.Add(bank);
        }

        public static (UserBanksDomain UserBanksDomain, string Message) Create(int idUser, List<BankDomain> banks)
        {
            var message = string.Empty;
            var userBanks = new UserBanksDomain(idUser, banks);

            return (userBanks, message);
        }
    }
}
