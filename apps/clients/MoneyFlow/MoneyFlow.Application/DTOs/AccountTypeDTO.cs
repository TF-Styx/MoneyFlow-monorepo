using MoneyFlow.Application.DTOs.BaseDTOs;
using MoneyFlow.Shared.Constants;

namespace MoneyFlow.Application.DTOs
{
    public class AccountTypeDTO : BaseDTO<AccountTypeDTO>
    {
        public int IdAccountType { get; set; }
        public string? AccountTypeName { get; set; }

        public int Index { get; set; }
    }
}
