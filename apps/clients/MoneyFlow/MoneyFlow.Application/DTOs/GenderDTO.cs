using MoneyFlow.Application.DTOs.BaseDTOs;

namespace MoneyFlow.Application.DTOs
{
    public class GenderDTO : BaseDTO<GenderDTO>
    {
        public int IdGender { get; set; }
        public string? GenderName { get; set; }
    }
}
