using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.DomainModels;
using MoneyFlow.Domain.Interfaces.Repositories;
using MoneyFlow.Infrastructure.Context;
using MoneyFlow.Infrastructure.EntityModel;

namespace MoneyFlow.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly Func<ContextMF> _factory;

        public CategoryRepository(Func<ContextMF> factory)
        {
            _factory = factory;
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        //var entity = new Category()
        //{
        //    CategoryName = categoryName,
        //    Description = description,
        //    Color = color,
        //    Image = image,
        //    IdUser = idUser
        //};

        //await context.AddAsync(entity);
        //await context.SaveChangesAsync();


        //await context.Subcategories.AddAsync(new Subcategory
        //{
        //    SubcategoryName = "Отсутствует",
        //    IdUser = idUser
        //});
        //await context.SaveChangesAsync();

        //var category = await context.Categories.FirstOrDefaultAsync(x => x.IdCategory == entity.IdCategory);
        //var subcategory = await context.Subcategories.FirstOrDefaultAsync(x => x.IdUser == idUser);

        //var catLinkSub = await context.CatLinkSubs.AddAsync(new CatLinkSub
        //{
        //    IdCategory = category.IdCategory,
        //    IdSubcategory = subcategory.IdSubcategory,
        //    CreatedAt = DateTime.Now,
        //    UpdatedAt = DateTime.Now,
        //    IdUser = idUser
        //});

        public async Task<int> CreateAsync(string? categoryName, string? description, string? color, byte[]? image, int idUser)
        {
            using (var context = _factory())
            {
                var createdCategory = new Category()
                {
                    CategoryName = categoryName,
                    Description = description,
                    Color = color,
                    Image = image,
                    IdUser = idUser
                };
                await context.Categories.AddAsync(createdCategory);
                await context.SaveChangesAsync();

                var categoryEntity = await context.Categories.Where(x => x.IdUser == idUser).OrderByDescending(x => x.IdCategory).FirstOrDefaultAsync();

                var createdDefaultSubcategory = new Subcategory
                {
                    SubcategoryName = $"Отсутствует ({categoryEntity.CategoryName})",
                    IdUser = idUser
                };
                await context.Subcategories.AddAsync(createdDefaultSubcategory);
                await context.SaveChangesAsync();

                var subcategoryEntity = await context.Subcategories.Where(x => x.IdUser == idUser).OrderByDescending(x => x.IdSubcategory).FirstOrDefaultAsync();

                var linkEntity = new CatLinkSub
                {
                    IdCategory = categoryEntity.IdCategory,
                    IdSubcategory = subcategoryEntity.IdSubcategory,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IdUser = idUser
                };                
                await context.CatLinkSubs.AddAsync(linkEntity);
                await context.SaveChangesAsync();

                var idCategory = await context.Categories.FirstOrDefaultAsync(x => x.CategoryName == categoryName);

                return idCategory.IdCategory;
            }
        }
        public int Create(string? categoryName, string? description, string? color, byte[]? image, int idUser)
        {
            using (var context = _factory())
            {
                var entity = new Category()
                {
                    CategoryName = categoryName,
                    Description = description,
                    Color = color,
                    Image = image,
                    IdUser = idUser
                };

                context.Add(entity);
                context.SaveChanges();

                return context.Categories.FirstOrDefault(x => x.CategoryName == categoryName).IdCategory;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<List<CategoryDomain>> GetAllAsync()
        {
            using (var context = _factory())
            {
                var list = new List<CategoryDomain>();
                var entity = await context.Categories.ToListAsync();

                foreach (var item in entity)
                {
                    list.Add(CategoryDomain.Create(item.IdCategory, item.CategoryName, item.Description, item.Color, item.Image, item.IdUser).CategoryDomain);
                }

                return list;
            }
        }
        public List<CategoryDomain> GetAll()
        {
            using (var context = _factory())
            {
                var list = new List<CategoryDomain>();
                var entity = context.Categories.ToList();

                foreach (var item in entity)
                {
                    list.Add(CategoryDomain.Create(item.IdCategory, item.CategoryName, item.Description, item.Color, item.Image, item.IdUser).CategoryDomain);
                }

                return list;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public int GetIdCat(int idUser)
        {
            using (var context = _factory())
            {
                return context.Categories.FirstOrDefault(x => x.IdUser == idUser).IdCategory;
            }
        }
        public int GetIdSubCat(int idUser, int idSub)
        {
            using (var context = _factory())
            {
                var idCategory = context.Categories.FirstOrDefault(x => x.IdUser == idUser).IdCategory;
                var idSubcategory = context.CatLinkSubs.FirstOrDefault(x => x.IdCategory == idCategory).IdSubcategory;

                return idSubcategory;
            }
        }

        //public async Task<CategoryDomain> GetIdCatAsync(int idUser)
        //{
        //    using (var context = _factory())
        //    {
        //        var entity = context.Categories.FirstOrDefault(x => x.IdUser == idUser);
        //        var domain = CategoryDomain.Create(entity.IdCategory, entity.CategoryName, entity.Description, entity.Color, entity.Image, entity.IdUser).CategoryDomain;

        //        return domain;
        //    }
        //}
        //public CategoryDomain GetIdCat(int idUser)
        //{
        //    using (var context = _factory())
        //    {
        //        var entity = context.Categories.FirstOrDefault(x => x.IdUser == idUser);
        //        var domain = CategoryDomain.Create(entity.IdCategory, entity.CategoryName, entity.Description, entity.Color, entity.Image, entity.IdUser).CategoryDomain;

        //        return domain;
        //    }
        //}

        public async Task<List<CategoryDomain>> GetCatAsync(int idUser)
        {
            using (var context = _factory())
            {
                var list = new List<CategoryDomain>();
                var entity = await context.Categories.Where(x => x.IdUser == idUser).ToListAsync();

                foreach (var item in entity)
                {
                    list.Add(CategoryDomain.Create(item.IdCategory, item.CategoryName, item.Description, item.Color, item.Image, item.IdUser).CategoryDomain);
                }

                return list;
            }
        }
        public List<CategoryDomain> GetCat(int idUser)
        {
            using (var context = _factory())
            {
                var list = new List<CategoryDomain>();
                var entity = context.Categories.Where(x => x.IdUser == idUser).ToList();

                foreach (var item in entity)
                {
                    list.Add(CategoryDomain.Create(item.IdCategory, item.CategoryName, item.Description, item.Color, item.Image, item.IdUser).CategoryDomain);
                }

                return list;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<CategoryDomain> GetAsync(int idCategory)
        {
            using (var context = _factory())
            {
                var entity = await context.Categories.FirstOrDefaultAsync(x => x.IdCategory == idCategory);
                var domain = CategoryDomain.Create(entity.IdCategory, entity.CategoryName, entity.Description, entity.Color, entity.Image, entity.IdUser).CategoryDomain;

                return domain;
            }
        }
        public CategoryDomain Get(int idCategory)
        {
            using (var context = _factory())
            {
                var entity = context.Categories.FirstOrDefault(x => x.IdCategory == idCategory);
                var domain = CategoryDomain.Create(entity.IdCategory, entity.CategoryName, entity.Description, entity.Color, entity.Image, entity.IdUser).CategoryDomain;

                return domain;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<int?> GetById(int idFinancialRecord)
        {
            using (var context = _factory())
            {
                var entity = await context.FinancialRecords.FirstOrDefaultAsync(x => x.IdFinancialRecord == idFinancialRecord);

                return entity.IdCategory;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<CategoryDomain> GetAsync(string categoryName)
        {
            using (var context = _factory())
            {
                var entity = await context.Categories.FirstOrDefaultAsync(x => x.CategoryName == categoryName);
                var domain = CategoryDomain.Create(entity.IdCategory, entity.CategoryName, entity.Description, entity.Color, entity.Image, entity.IdUser).CategoryDomain;

                return domain;
            }
        }
        public CategoryDomain Get(string categoryName)
        {
            using (var context = _factory())
            {
                var entity = context.Categories.FirstOrDefault(x => x.CategoryName == categoryName);
                var domain = CategoryDomain.Create(entity.IdCategory, entity.CategoryName, entity.Description, entity.Color, entity.Image, entity.IdUser).CategoryDomain;

                return domain;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<List<CategoryWithSubcategoryDomain>> GetCategoryWithSubcategoryAsync(int idUser)
        {
            using (var context = _factory())
            {
                // Получаем все категории, связанные с пользователем.
                var categories = await context.Categories
                    .Where(x => x.IdUser == idUser)
                    .Distinct()
                    .ToListAsync();

                // Получаем все подкатегории, связанные с пользователем.
                var subcategories = await context.CatLinkSubs
                    .Include(x => x.IdSubcategoryNavigation)
                    .Where(x => x.IdUser == idUser)
                    .ToListAsync();

                var categoriesWithSubcategories = categories.Select(category =>
                {
                    var categorySubcategories = subcategories
                        .Where(x => x.IdCategory == category.IdCategory)
                        .Select(x => SubcategoryDomain.Create(
                            x.IdSubcategoryNavigation.IdSubcategory,
                            x.IdSubcategoryNavigation.SubcategoryName,
                            x.IdSubcategoryNavigation.Description,
                            x.IdSubcategoryNavigation.Image,
                            x.IdSubcategoryNavigation.IdUser
                        ).SubcategoryDomain)
                        .ToList();

                    return CategoryWithSubcategoryDomain.Create(
                        CategoryDomain.Create(
                            category.IdCategory,
                            category.CategoryName,
                            category.Description,
                            category.Color,
                            category.Image,
                            category.IdUser
                        ).CategoryDomain,
                        categorySubcategories
                    ).CategoryWithSubcategoryDomain;
                }).ToList();

                return categoriesWithSubcategories;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<int> UpdateAsync(int idCategory, string? categoryName, string? description, string? color, byte[]? image, int idUser)
        {
            using (var context = _factory())
            {
                var entity = await context.Categories.FirstOrDefaultAsync(x => x.IdCategory == idCategory);

                entity.CategoryName = categoryName;
                entity.Description = description;
                entity.Color = color;
                entity.Image = image;
                entity.IdUser = idUser;

                context.Categories.Update(entity);
                context.SaveChanges();

                return idCategory;
            }
        }
        public int Update(int idCategory, string? categoryName, string? description, string? color, byte[]? image, int idUser)
        {
            using (var context = _factory())
            {
                var entity = context.Categories.FirstOrDefault(x => x.IdCategory == idCategory);

                entity.CategoryName = categoryName;
                entity.Description = description;
                entity.Color = color;
                entity.Image = image;
                entity.IdUser = idUser;

                context.Categories.Update(entity);
                context.SaveChanges();

                return idCategory;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<(bool catLinkSub, bool financialRecord)> ExistRelatedDataAsync(int idCategory)
        {
            using (var context = _factory())
            {
                bool idUsedCatLinkSub = await context.CatLinkSubs.AnyAsync(x => x.IdCategory == idCategory);
                bool idUsedFinancialRecord = await context.FinancialRecords.AnyAsync(x => x.IdCategory == idCategory);

                return (idUsedCatLinkSub, idUsedFinancialRecord);
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<int> DeleteAsync(int idCategory)
        {
            using (var context = _factory())
            {
                await context.CatLinkSubs.Where(x => x.IdCategory == idCategory).ExecuteDeleteAsync();
                var cat = await context.Categories.FirstOrDefaultAsync(x => x.IdCategory == idCategory);

                context.Remove(cat);
                context.SaveChanges();

                return cat.IdCategory;
            }
        }
        public int Delete(int idCategory)
        {
            return Task.Run(() => DeleteAsync(idCategory)).Result;
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
