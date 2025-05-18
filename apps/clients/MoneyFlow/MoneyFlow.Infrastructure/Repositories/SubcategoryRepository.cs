using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.DomainModels;
using MoneyFlow.Domain.Interfaces.Repositories;
using MoneyFlow.Infrastructure.Context;
using MoneyFlow.Infrastructure.EntityModel;

namespace MoneyFlow.Infrastructure.Repositories
{
    public class SubcategoryRepository : ISubcategoryRepository
    {
        private readonly Func<ContextMF> _factory;

        public SubcategoryRepository(Func<ContextMF> factory)
        {
            _factory = factory;
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<int> CreateAsync(string? subcategoryName, string? description, byte[]? image, int idUser)
        {
            using (var context = _factory())
            {
                var entity = new Subcategory()
                {
                    SubcategoryName = subcategoryName,
                    Description = description,
                    Image = image,
                    IdUser = idUser
                };

                await context.AddAsync(entity);
                await context.SaveChangesAsync();

                return await context.Subcategories.Where(x => x.IdUser == idUser).OrderByDescending(x => x.IdSubcategory).Select(x => x.IdSubcategory).FirstOrDefaultAsync();
            }
        }
        public int Create(string? subcategoryName, string? description, byte[]? image, int idUser)
        {
            return Task.Run(() => CreateAsync(subcategoryName, description, image, idUser)).Result;
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<List<SubcategoryDomain>> GetAllAsync()
        {
            using (var context = _factory())
            {
                var list = new List<SubcategoryDomain>();
                var entity = await context.Subcategories.ToListAsync();

                foreach (var item in entity)
                {
                    list.Add(SubcategoryDomain.Create(item.IdSubcategory, item.SubcategoryName, item.Description, item.Image, item.IdUser).SubcategoryDomain);
                }

                return list;
            }
        }
        public List<SubcategoryDomain> GetAll()
        {
            using (var context = _factory())
            {
                var list = new List<SubcategoryDomain>();
                var entity = context.Subcategories.ToList();

                foreach (var item in entity)
                {
                    list.Add(SubcategoryDomain.Create(item.IdSubcategory, item.SubcategoryName, item.Description, item.Image, item.IdUser).SubcategoryDomain);
                }

                return list;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public List<SubcategoryDomain> GetAllIdUserSub(int idUser)
        {
            using (var context = _factory())
            {
                var list = new List<SubcategoryDomain>();
                var entity = context.CatLinkSubs.Where(x => x.IdUser == idUser).Include(x => x.IdSubcategoryNavigation);

                foreach (var item in entity)
                {
                    list.Add(SubcategoryDomain.Create(item.IdSubcategoryNavigation.IdSubcategory, item.IdSubcategoryNavigation.SubcategoryName, item.IdSubcategoryNavigation.Description, item.IdSubcategoryNavigation.Image, item.IdUser).SubcategoryDomain);
                }

                return list;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public List<SubcategoryDomain> GetIdUserIdCategorySub(int idUser, int idCategory)
        {
            using (var context = _factory())
            {
                var list = new List<SubcategoryDomain>();
                var entity = context.CatLinkSubs.Where(x => x.IdUser == idUser && x.IdCategory == idCategory).Include(x => x.IdSubcategoryNavigation);

                foreach (var item in entity)
                {
                    list.Add(SubcategoryDomain.Create(item.IdSubcategoryNavigation.IdSubcategory, item.IdSubcategoryNavigation.SubcategoryName, item.IdSubcategoryNavigation.Description, item.IdSubcategoryNavigation.Image, item.IdUser).SubcategoryDomain);
                }

                return list;
            }
        }
        
        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<SubcategoryDomain> GetByIdSub(int idUser, int idCategory)
        {
            using (var context = _factory())
            {
                var subcategory = await context.CatLinkSubs.Include(x => x.IdSubcategoryNavigation)
                    .Where(x => x.IdUser == idUser)
                    .Where(x => x.IdCategory == idCategory).FirstAsync();

                var sub = SubcategoryDomain.Create(subcategory.IdSubcategoryNavigation.IdSubcategory, subcategory.IdSubcategoryNavigation.SubcategoryName, subcategory.IdSubcategoryNavigation.Description, subcategory.IdSubcategoryNavigation.Image, subcategory.IdUser);

                return sub.SubcategoryDomain;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<SubcategoryDomain> GetAsync(int idSubcategory)
        {
            using (var context = _factory())
            {
                var entity = await context.Subcategories.FirstOrDefaultAsync(x => x.IdSubcategory == idSubcategory);
                var domain = SubcategoryDomain.Create(entity.IdSubcategory, entity.SubcategoryName, entity.Description, entity.Image, entity.IdUser).SubcategoryDomain;

                return domain;
            }
        }
        public SubcategoryDomain Get(int idSubcategory)
        {
            using (var context = _factory())
            {
                var entity = context.Subcategories.FirstOrDefault(x => x.IdSubcategory == idSubcategory);
                var domain = SubcategoryDomain.Create(entity.IdSubcategory, entity.SubcategoryName, entity.Description, entity.Image, entity.IdUser).SubcategoryDomain;

                return domain;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<int?> GetById(int idFinancialRecord)
        {
            using (var context = _factory())
            {
                var entity = await context.FinancialRecords.FirstOrDefaultAsync(x => x.IdFinancialRecord == idFinancialRecord);

                return entity.IdSubcategory;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<SubcategoryDomain> GetAsync(string subcategoryName)
        {
            using (var context = _factory())
            {
                var entity = await context.Subcategories.FirstOrDefaultAsync(x => x.SubcategoryName == subcategoryName);
                var domain = SubcategoryDomain.Create(entity.IdSubcategory, entity.SubcategoryName, entity.Description, entity.Image, entity.IdUser).SubcategoryDomain;

                return domain;
            }
        }
        public SubcategoryDomain Get(string subcategoryName)
        {
            using (var context = _factory())
            {
                var entity = context.Subcategories.FirstOrDefault(x => x.SubcategoryName == subcategoryName);
                var domain = SubcategoryDomain.Create(entity.IdSubcategory, entity.SubcategoryName, entity.Description, entity.Image, entity.IdUser).SubcategoryDomain;

                return domain;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<int> UpdateAsync(int idSubcategory, string? subcategoryName, string? description, byte[]? image)
        {
            using (var context = _factory())
            {
                var entity = await context.Subcategories.FirstOrDefaultAsync(x => x.IdSubcategory == idSubcategory);

                entity.SubcategoryName = subcategoryName;
                entity.Description = description;
                entity.Image = image;

                context.Subcategories.Update(entity);
                context.SaveChanges();

                return idSubcategory;
            }
        }
        public int Update(int idSubcategory, string? subcategoryName, string? description, byte[]? image)
        {
            using (var context = _factory())
            {
                var entity = context.Subcategories.FirstOrDefault(x => x.IdSubcategory == idSubcategory);

                entity.SubcategoryName = subcategoryName;
                entity.Description = description;
                entity.Image = image;

                context.Subcategories.Update(entity);
                context.SaveChanges();

                return idSubcategory;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<(bool catLinkSub, bool financialRecord)> ExistRelatedDataAsync(int idSubcategory)
        {
            using (var context = _factory())
            {
                bool idUsedCatLinkSub = await context.CatLinkSubs.AnyAsync(x => x.IdSubcategory == idSubcategory);
                bool idUsedFinancialRecord = await context.FinancialRecords.AnyAsync(x => x.IdSubcategory == idSubcategory);

                return (idUsedCatLinkSub, idUsedFinancialRecord);
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<List<int>> DeleteAsync(int idSubcategory)
        {
            var ids = new List<int>();

            using (var context = _factory())
            {
                var models = await context.Subcategories.Where(x => x.IdSubcategory == idSubcategory).ToListAsync();
                ids.AddRange(models.Select(x => x.IdSubcategory));

                context.RemoveRange(models);
                context.SaveChanges();

                return ids;
            }
        }
        public List<int> Delete(int idSubcategory)
        {
            var ids = new List<int>();

            using (var context = _factory())
            {
                var models = context.Subcategories.Where(x => x.IdSubcategory == idSubcategory).ToList();
                ids.AddRange(models.Select(x => x.IdSubcategory));

                context.RemoveRange(models);
                context.SaveChanges();

                return ids;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
