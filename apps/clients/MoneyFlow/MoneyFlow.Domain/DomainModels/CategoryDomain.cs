using MoneyFlow.Shared.Constants;

namespace MoneyFlow.Domain.DomainModels
{
    public class CategoryDomain
    {
        private CategoryDomain(int idCategory, string? categoryName, string? description, string? color, byte[]? image, int idUser)
        {
            IdCategory = idCategory;
            CategoryName = categoryName;
            Description = description;
            Color = color;
            Image = image;
            IdUser = idUser;
        }

        public int IdCategory { get; private set; }
        public string? CategoryName { get; private set; }
        public string? Description { get; private set; }
        public string? Color { get; private set; }
        public byte[]? Image { get; private set; }
        public int IdUser { get; private set; }

        public static (CategoryDomain CategoryDomain, string Message) Create(int idCategory, string? categoryName, string? description, string? color, byte[]? image, int idUser)
        {
            var message = string.Empty;

            //if (categoryName.Length > IntConstants.MAX_SUBCATEGORYNAME_LENGHT)
            //{
            //    return (null, "Превышена допустимая длина в «255» символов");
            //}

            var category = new CategoryDomain(idCategory, categoryName, description, color, image, idUser);

            return (category, message);
        }
    }
}
