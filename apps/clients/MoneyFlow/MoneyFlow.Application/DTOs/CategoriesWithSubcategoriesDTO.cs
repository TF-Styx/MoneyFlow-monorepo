using System.Collections.ObjectModel;

namespace MoneyFlow.Application.DTOs
{
    public class CategoriesWithSubcategoriesDTO
    {
        public CategoryDTO Category { get; set; }
        public ObservableCollection<SubcategoryDTO> Subcategories { get; set; }

        public int Index { get; set; }
    }
}
