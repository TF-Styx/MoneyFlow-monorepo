using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.DomainModels;
using MoneyFlow.Domain.Interfaces.Repositories;
using MoneyFlow.Infrastructure.Context;
using MoneyFlow.Infrastructure.EntityModel;

namespace MoneyFlow.Infrastructure.Repositories
{
    public class FinancialRecordRepository : IFinancialRecordRepository
    {
        private readonly Func<ContextMF> _factory;

        public FinancialRecordRepository(Func<ContextMF> factory)
        {
            _factory = factory;
        }

        #region Create

        public async Task<int> CreateAsync(string? recordName, decimal? amount, string? description, int? idTransactionType, int? idUser, int? idCategory, int? idSubcategory, int? idAccount, DateTime? date)
        {
            using (var context = _factory())
            {
                var entity = new FinancialRecord()
                {
                    RecordName = recordName,
                    Ammount = amount,
                    Description = description,
                    IdTransactionType = idTransactionType,
                    IdUser = idUser,
                    IdCategory = idCategory,
                    IdSubcategory = idSubcategory,
                    IdAccount = idAccount,
                    Date = date
                };

                var account = await context.Accounts.FindAsync(idAccount);

                if (idTransactionType == 2)
                {
                    account.Balance -= amount;
                }
                else if (idTransactionType == 1)
                {
                    account.Balance += amount;
                }

                await context.AddAsync(entity);
                await context.SaveChangesAsync();

                return await context.FinancialRecords
                    .Where(x => x.IdUser == idUser)
                    .OrderByDescending(x => x.IdFinancialRecord)
                    .Select(x => x.IdFinancialRecord)
                    .FirstOrDefaultAsync();
            }
        }

        #endregion

        #region GetFinancialRecordDomain

        public async Task<List<FinancialRecordDomain>> GetAllAsync(int idUser)
        {
            using (var context = _factory())
            {
                var list = new List<FinancialRecordDomain>();
                var entity = await context.FinancialRecords.Where(x => x.IdUser == idUser).ToListAsync();

                foreach (var item in entity)
                {
                    list.Add(FinancialRecordDomain.Create(item.IdFinancialRecord, item.RecordName, item.Ammount, item.Description, item.IdTransactionType, item.IdUser, item.IdCategory, item.IdSubcategory, item.IdAccount, item.Date).FinancialRecordDomain);
                }

                return list;
            }
        }
        public List<FinancialRecordDomain> GetAll(int idUser)
        {
            using (var context = _factory())
            {
                var list = new List<FinancialRecordDomain>();
                var entity = context.FinancialRecords.Where(x => x.IdUser == idUser).ToList();

                foreach (var item in entity)
                {
                    list.Add(FinancialRecordDomain.Create(item.IdFinancialRecord, item.RecordName, item.Ammount, item.Description, item.IdTransactionType, item.IdUser, item.IdCategory, item.IdSubcategory, item.IdAccount, item.Date).FinancialRecordDomain);
                }

                return list;
            }
        }

        public async Task<FinancialRecordDomain> GetAsync(int idFinancialRecord)
        {
            using (var context = _factory())
            {
                var entity = await context.FinancialRecords.FirstOrDefaultAsync(x => x.IdFinancialRecord == idFinancialRecord);
                var domain = FinancialRecordDomain.Create(entity.IdFinancialRecord, entity.RecordName, entity.Ammount, entity.Description, entity.IdTransactionType, entity.IdUser, entity.IdCategory, entity.IdSubcategory, entity.IdAccount, entity.Date).FinancialRecordDomain;

                return domain;
            }
        }
        public FinancialRecordDomain Get(int idFinancialRecord)
        {
            using (var context = _factory())
            {
                var entity = context.FinancialRecords.FirstOrDefault(x => x.IdFinancialRecord == idFinancialRecord);
                var domain = FinancialRecordDomain.Create(entity.IdFinancialRecord, entity.RecordName, entity.Ammount, entity.Description, entity.IdTransactionType, entity.IdUser, entity.IdCategory, entity.IdSubcategory, entity.IdAccount, entity.Date).FinancialRecordDomain;

                return domain;
            }
        }

        #endregion

        #region GetFinancialRecordViewingDomain

        public async Task<List<FinancialRecordViewingDomain>> GetAllViewingAsync(int idUser, FinancialRecordFilterDomain filter)
        {
            using (var context = _factory())
            {
                var query = context.FinancialRecords
                    .AsNoTracking()
                        .Include(x => x.IdUserNavigation)
                        .Include(x => x.IdTransactionTypeNavigation)
                        .Include(x => x.IdCategoryNavigation)
                        .Include(x => x.IdSubcategoryNavigation)
                        .Include(x => x.IdAccountNavigation)
                            .Where(x => x.IdUser == idUser)
                                .AsSplitQuery()
                                    .AsQueryable();

                if (filter.IsConsiderAmount == true)
                    query = query.Where(x => x.Ammount >= filter.AmountStart && x.Ammount <= filter.AmountEnd);

                if (filter.IdTransactionType.HasValue)
                    query = query.Where(x => x.IdTransactionType == filter.IdTransactionType.Value);

                if (filter.IdCategory.HasValue)
                    query = query.Where(x => x.IdCategory == filter.IdCategory.Value);

                if (filter.IdSubcategory.HasValue)
                    query = query.Where(x => x.IdSubcategory == filter.IdSubcategory.Value);

                if (filter.IdAccount.HasValue)
                    query = query.Where(x => x.IdAccount == filter.IdAccount.Value);

                if (filter.IsConsiderDate == true)
                    query = query.Where(x => x.Date >= filter.DateStart && x.Date <= filter.DateEnd);

                var list = await query.Select(x => FinancialRecordViewingDomain.Create
                    (
                        x.IdFinancialRecord,
                        x.RecordName,
                        x.Ammount,
                        x.Description,
                        x.IdTransactionType,
                        x.IdTransactionTypeNavigation.TransactionTypeName,
                        idUser,
                        x.IdCategory,
                        x.IdCategoryNavigation.CategoryName,
                        x.IdSubcategory,
                        x.IdSubcategoryNavigation.SubcategoryName,
                        x.IdAccountNavigation.NumberAccount,
                        x.Date
                    ).FinancialRecordViewingDomain).ToListAsync();

                return list;

                //query = query.OrderByDescending(x => x.IdFinancialRecord);

                //var list = new List<FinancialRecordViewingDomain>();

                //foreach (var item in await query.ToListAsync())
                //{
                //    list.Add(FinancialRecordViewingDomain.Create
                //    (
                //        item.IdFinancialRecord,
                //        item.RecordName,
                //        item.Ammount,
                //        item.Description,
                //        item.IdTransactionTypeNavigation.TransactionTypeName,
                //        idUser,
                //        item.IdCategoryNavigation.CategoryName,
                //        new List<string>(),
                //        item.IdAccountNavigation.NumberAccount,
                //        item.Date
                //    ).FinancialRecordViewingDomain);
                //}

                //return list;
            }
        }

        public List<FinancialRecordViewingDomain> GetAllViewing(int idUser, FinancialRecordFilterDomain filter)
        {
            return Task.Run(() => GetAllViewingAsync(idUser, filter)).Result;
        }

        public async Task<FinancialRecordViewingDomain> GetViewingAsync(int idFinancialRecord)
        {
            using (var context = _factory())
            {
                var entity = await context.FinancialRecords
                    .AsNoTracking()
                        .Include(x => x.IdUserNavigation)
                        .Include(x => x.IdTransactionTypeNavigation)
                        .Include(x => x.IdCategoryNavigation)
                        .Include(x => x.IdSubcategoryNavigation)
                        .Include(x => x.IdAccountNavigation)
                            .FirstOrDefaultAsync(x => x.IdFinancialRecord == idFinancialRecord);

                var domain = FinancialRecordViewingDomain.Create
                    (
                        idFinancialRecord,
                        entity.RecordName,
                        entity.Ammount,
                        entity.Description,
                        entity.IdTransactionType,
                        entity.IdTransactionTypeNavigation?.TransactionTypeName,
                        entity.IdUser,
                        entity.IdCategory,
                        entity.IdCategoryNavigation?.CategoryName,
                        entity.IdSubcategory,
                        entity.IdSubcategoryNavigation?.SubcategoryName,
                        entity.IdAccountNavigation?.NumberAccount,
                        entity.Date
                    ).FinancialRecordViewingDomain;

                return domain;
            }
        }

        public FinancialRecordViewingDomain GetViewing(int idFinancialRecord)
        {
            return Task.Run(() => GetViewingAsync(idFinancialRecord)).Result;
        }

        public async Task<FinancialRecordViewingDomain> GetByIdAsync(int idUser, int idFinancialRecord, int? idCategory, int? idSubcategory)
        {
            using (var context = _factory())
            {
                var entity = await context.FinancialRecords
                    .AsNoTracking()
                        .Include(x => x.IdUserNavigation)
                        .Include(x => x.IdTransactionTypeNavigation)
                        .Include(x => x.IdCategoryNavigation)
                        .Include(x => x.IdSubcategoryNavigation)
                        .Include(x => x.IdAccountNavigation)
                            .FirstOrDefaultAsync(x => x.IdFinancialRecord == idFinancialRecord);

                var domain = FinancialRecordViewingDomain.Create
                    (
                        entity.IdFinancialRecord,
                        entity.RecordName,
                        entity.Ammount,
                        entity.Description,
                        entity.IdTransactionType,
                        entity.IdTransactionTypeNavigation.TransactionTypeName,
                        idUser,
                        entity.IdCategory, 
                        entity.IdCategoryNavigation.CategoryName, 
                        entity.IdSubcategory,
                        entity.IdSubcategoryNavigation.SubcategoryName,
                        entity.IdAccountNavigation.NumberAccount,
                        entity.Date
                    ).FinancialRecordViewingDomain;

                return domain;
            }
        }
        public FinancialRecordViewingDomain GetById(int idUser, int idFinancialRecord, int? idCategory, int? idSubcategory)
        {
            return Task.Run(() => GetByIdAsync(idUser, idFinancialRecord, idCategory, idSubcategory)).Result;
        }

        #endregion

        #region Update

        public async Task<int> UpdateAsync(int idFinancialRecord, string? recordName, decimal? amount, string? description, int? idTransactionType, int? idUser, int? idCategory, int? idSubcategory, int? idAccount, DateTime? date)
        {
            using (var context = _factory())
            {
                var entity = await context.FinancialRecords.FirstOrDefaultAsync(x => x.IdFinancialRecord == idFinancialRecord);

                entity.RecordName = recordName;
                entity.Ammount = amount;
                entity.Description = description;
                entity.IdTransactionType = idTransactionType;
                entity.IdUser = idUser;
                entity.IdCategory = idCategory;
                entity.IdAccount = idAccount;
                entity.Date = date;

                context.Update(entity);
                context.SaveChanges();

                return idFinancialRecord;
            }
        }
        public int Update(int idFinancialRecord, string? recordName, decimal? amount, string? description, int? idTransactionType, int? idUser, int? idCategory, int? idSubcategory, int? idAccount, DateTime? date)
        {
            using (var context = _factory())
            {
                var entity = context.FinancialRecords.FirstOrDefault(x => x.IdFinancialRecord == idFinancialRecord);

                entity.RecordName = recordName;
                entity.Ammount = amount;
                entity.Description = description;
                entity.IdTransactionType = idTransactionType;
                entity.IdUser = idUser;
                entity.IdCategory = idCategory;
                entity.IdAccount = idAccount;
                entity.Date = date;

                context.Update(entity);
                context.SaveChanges();

                return idFinancialRecord;
            }
        }

        #endregion

        #region Delete

        public async Task<List <int>> DeleteListAsync(int id, bool isDeleteByIdCategory)
        {
            var ids = new List<int>();

            using (var context = _factory())
            {
                if (isDeleteByIdCategory)
                {
                    var models = await context.FinancialRecords.Where(x => x.IdCategory == id).ToListAsync();
                    ids.AddRange(models.Select(x => x.IdFinancialRecord));

                    context.RemoveRange(models);
                    context.SaveChanges();
                }
                else
                {
                    var models = await context.FinancialRecords.Where(x => x.IdSubcategory == id).ToListAsync();
                    ids.AddRange(models.Select(x => x.IdFinancialRecord));

                    context.RemoveRange(models);
                    context.SaveChanges();
                }

                return ids;
            }
        }

        public async Task<int> DeleteAsync(int idFinancialRecord)
        {
            using (var context = _factory())
            {
                var id = await context.FinancialRecords.FirstOrDefaultAsync(x => x.IdFinancialRecord == idFinancialRecord);

                return id.IdFinancialRecord;
            }
        }
        public int Delete(int idFinancialRecord)
        {
            using (var context = _factory())
            {
                var id = context.FinancialRecords.FirstOrDefault(x => x.IdFinancialRecord == idFinancialRecord);

                return id.IdFinancialRecord;
            }
        }

        #endregion
    }
}
