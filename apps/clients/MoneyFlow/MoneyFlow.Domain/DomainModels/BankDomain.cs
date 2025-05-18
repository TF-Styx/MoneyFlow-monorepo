using MoneyFlow.Shared.Constants;

namespace MoneyFlow.Domain.DomainModels
{
    public class BankDomain
    {
        private BankDomain(int idBank, string? bankName)
        {
            IdBank = idBank;
            BankName = bankName;
        }

        public int IdBank { get; private set; }
        public string? BankName { get; private set; }

        public static (BankDomain BankDomain, string Message) Create(int idBank, string? bankName)
        {
            var message = string.Empty;

            if (string.IsNullOrWhiteSpace(bankName)) { return (null, "вы не заполнили поля!!"); }

            if (bankName.Length > IntConstants.MAX_BANK_NAME_LENGHT) { return (null, "Превышена длина слова в «255» символов!!"); }

            var bank = new BankDomain(idBank, bankName);

            return (bank, message);
        }
    }
}
