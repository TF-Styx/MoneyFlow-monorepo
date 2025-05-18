using Microsoft.EntityFrameworkCore;
using MoneyFlow.MVVM.Models.DB_MSSQL;

namespace MoneyFlow.Utils.Helpers
{
    public static class DataBaseHelper
    {
        public static async Task<List<Subcategory>> GetSubcategoriesByUserIdAsync(int idUser)
        {
            using (var context = new MoneyFlowDbContext())
            {
                var subcategories = await context.Subcategories
                    .Where(sub => context.Categories
                        .Any(cat => cat.IdCategory == sub.IdSubcategory && cat.IdUser == idUser))
                    .ToListAsync();

                return subcategories;
            }
        }
    }
}
