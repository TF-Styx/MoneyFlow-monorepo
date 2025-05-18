namespace MoneyFlow.Domain.DomainModels
{
    public class CategoryWithSubcategoryDomain
    {
        private CategoryWithSubcategoryDomain(CategoryDomain category, List<SubcategoryDomain> subcategories)
        {
            Category = category;
            Subcategories = subcategories;
        }

        public CategoryDomain Category { get; private set; }
        public List<SubcategoryDomain> Subcategories { get; private set; }

        public static (CategoryWithSubcategoryDomain CategoryWithSubcategoryDomain, string Message) Create(CategoryDomain category, List<SubcategoryDomain> subcategories)
        {
            string message = string.Empty;

            //if (subcategories.Count == 0)
            //{
            //    subcategories.Add(SubcategoryDomain.Create(-1, "Подкатегории отсутствуют!!", "", new byte[0], 0).SubcategoryDomain);
            //}

            var categoryWithSubcategoryDomain = new CategoryWithSubcategoryDomain(category, subcategories);

            return (categoryWithSubcategoryDomain, message);
        }
    }
}
