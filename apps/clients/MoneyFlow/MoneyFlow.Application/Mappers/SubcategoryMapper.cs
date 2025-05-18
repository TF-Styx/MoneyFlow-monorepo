using MoneyFlow.Application.DTOs;
using MoneyFlow.Domain.DomainModels;

namespace MoneyFlow.Application.Mappers
{
    public static class SubcategoryMapper
    {
        public static (SubcategoryDTO SubcategoryDTO, string Message) ToDTO(this SubcategoryDomain subcategory)
        {
            string message = string.Empty;

            if (subcategory == null) { return (null, "Данной под категории не найдено!!"); }

            var dto = new SubcategoryDTO()
            {
                IdSubcategory = subcategory.IdSubcategory,
                SubcategoryName = subcategory.SubcategoryName,
                Description = subcategory.Description,
                Image = subcategory.Image,
            };

            return (dto, message);
        }

        public static List<SubcategoryDTO> ToListDTO(this IEnumerable<SubcategoryDomain> subcategories)
        {
            var list = new List<SubcategoryDTO>();

            foreach (var item in subcategories)
            {
                list.Add(item.ToDTO().SubcategoryDTO);
            }

            return list;
        }
    }
}
