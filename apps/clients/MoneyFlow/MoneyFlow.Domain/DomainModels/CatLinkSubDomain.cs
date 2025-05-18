namespace MoneyFlow.Domain.DomainModels
{
    public class CatLinkSubDomain
    {
        private CatLinkSubDomain(int idUser, int idCategory, int idSubcategory)
        {
            IdUser = idUser;
            IdCategory = idCategory;
            IdSubcategory = idSubcategory;
        }

        public int IdUser { get; private set; }
        public int IdCategory { get; private set; }
        public int IdSubcategory { get; private set; }

        public static (CatLinkSubDomain CatLinkSubDomain, string Message) Create(int idUser, int idCategory, int idSubcategory)
        {
            string message = string.Empty;

            if (idUser == null && idCategory == null && idSubcategory == null)
            {
                return (null, "Ничего не найдено");
            }

            var catLinSub = new CatLinkSubDomain(idUser, idCategory, idSubcategory);

            return (catLinSub, message);
        }

        //public static (CategoryDomain CategoryDomain, SubcategoryDomain SubcategoryDomain, string Message) Create(int idUser, int idCategory, int idSubcategory)
        //{
        //    string message = string.Empty;

        //    if (idUser == null && idCategory == null && idSubcategory == null)
        //    {
        //        return (null, null, "Ничего не найдено");
        //    }

        //    var catLinSub = new 
        //}
    }
}
