using MoneyFlow.Application.DTOs.BaseDTOs;

namespace MoneyFlow.Application.DTOs
{
    public class BankDTO : BaseDTO<BankDTO>
    {
        public int IdBank { get; set; }
        public string? BankName { get; set; }

        public int Index { get; set; }
    }
}
