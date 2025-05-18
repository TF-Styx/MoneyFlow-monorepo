using MoneyFlow.Application.DTOs.BaseDTOs;

namespace MoneyFlow.Application.DTOs
{
    public class SubcategoryDTO : BaseDTO<SubcategoryDTO>
    {
        public int IdSubcategory { get; set; }
        public string? SubcategoryName { get; set; }
        public string? Description { get; set; }
        public byte[]? Image { get; set; }

        public int Index { get; set; }
    }
}
