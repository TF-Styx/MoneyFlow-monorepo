using MoneyFlow.Application.DTOs.BaseDTOs;

namespace MoneyFlow.Application.DTOs
{
    public class CategoryDTO : BaseDTO<CategoryDTO>
    {
        public int IdCategory { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        public byte[]? Image { get; set; }
        public int IdUser { get; set; }

        public int Index { get; set; }
    }
}
