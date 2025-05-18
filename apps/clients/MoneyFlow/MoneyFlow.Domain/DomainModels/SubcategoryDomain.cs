using MoneyFlow.Shared.Constants;

namespace MoneyFlow.Domain.DomainModels
{
    public class SubcategoryDomain
    {
        private SubcategoryDomain(int idSubcategory, string? subcategoryName, string? description, byte[] image, int idUser)
        {
            IdSubcategory = idSubcategory;
            SubcategoryName = subcategoryName;
            Description = description;
            Image = image;
            IdUser = idUser;
        }

        public int IdSubcategory { get; private set; }
        public string? SubcategoryName { get; private set; }
        public string? Description { get; private set; }
        public byte[]? Image { get; private set; }
        public int IdUser { get; private set; }

        public static (SubcategoryDomain SubcategoryDomain, string Message) Create(int idSubcategory, string? subcategoryName, string? description, byte[] image, int idUser)
        {
            var message = string.Empty;

            //if (string.IsNullOrWhiteSpace(subcategoryName))
            //{
            //    return (null, "Вы не заполнили поря!!");
            //}

            //if (subcategoryName.Length > IntConstants.MAX_SUBCATEGORYNAME_LENGHT)
            //{
            //    return (null, "Превышена допустимая длина в «255» символов");
            //}

            var subcategory = new SubcategoryDomain(idSubcategory, subcategoryName, description, image, idUser);

            return (subcategory, message);
        }
    }
}
