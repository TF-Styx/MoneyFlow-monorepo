using MoneyFlow.Application.DTOs.BaseDTOs;

namespace MoneyFlow.Application.DTOs
{
    public class AccountDTO : BaseDTO<AccountDTO>
    {
        public int IdAccount { get; set; }
        public int? NumberAccount { get; set; }
        public BankDTO Bank { get; set; }
        public AccountTypeDTO AccountType { get; set; }
        public decimal? Balance { get; set; }

        public int Index { get; set; }
    }
}
