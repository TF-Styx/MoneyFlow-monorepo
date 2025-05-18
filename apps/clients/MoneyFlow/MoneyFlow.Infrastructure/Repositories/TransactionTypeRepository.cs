using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.DomainModels;
using MoneyFlow.Domain.Interfaces.Repositories;
using MoneyFlow.Infrastructure.Context;
using MoneyFlow.Infrastructure.EntityModel;

namespace MoneyFlow.Infrastructure.Repositories
{
    public class TransactionTypeRepository : ITransactionTypeRepository
    {
        private readonly Func<ContextMF> _factory;

        public TransactionTypeRepository(Func<ContextMF> factory)
        {
            _factory = factory;
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<int> CreateAsync(string? transactionTypeName, string? description)
        {
            using (var context = _factory())
            {
                var transactionTypeEntity = new TransactionType()
                {
                    TransactionTypeName = transactionTypeName,
                    Description = description
                };

                await context.AddAsync(transactionTypeEntity);
                await context.SaveChangesAsync();

                return context.TransactionTypes.FirstOrDefault(x => x.TransactionTypeName == transactionTypeName).IdTransactionType;
            }
        }
        public int Create(string? transactionTypeName, string? description)
        {
            using (var context = _factory())
            {
                var transactionTypeEntity = new TransactionType()
                {
                    TransactionTypeName = transactionTypeName,
                    Description = description
                };

                context.Add(transactionTypeEntity);
                context.SaveChanges();

                return context.TransactionTypes.FirstOrDefault(x => x.TransactionTypeName == transactionTypeName).IdTransactionType;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<List<TransactionTypeDomain>> GetAllAsync()
        {
            using (var context = _factory())
            {
                var transactionTypeList = new List<TransactionTypeDomain>();
                var transactionTypeEntity = await context.TransactionTypes.ToListAsync();

                foreach (var item in transactionTypeEntity)
                {
                    transactionTypeList.Add(TransactionTypeDomain.Create(item.IdTransactionType, item.TransactionTypeName, item.Description).TransactionTypeDomain);
                }

                return transactionTypeList;
            }
        }
        public List<TransactionTypeDomain> GetAll()
        {
            using (var context = _factory())
            {
                var transactionTypeList = new List<TransactionTypeDomain>();
                var transactionTypeEntity = context.TransactionTypes.ToList();

                foreach (var item in transactionTypeEntity)
                {
                    transactionTypeList.Add(TransactionTypeDomain.Create(item.IdTransactionType, item.TransactionTypeName, item.Description).TransactionTypeDomain);
                }

                return transactionTypeList;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<TransactionTypeDomain> GetAsync(int idTransactionType)
        {
            using (var context = _factory())
            {
                var transactionTypeEntity = await context.TransactionTypes.FirstOrDefaultAsync(x => x.IdTransactionType == idTransactionType);
                var transactionTypeDomain = TransactionTypeDomain.Create(transactionTypeEntity.IdTransactionType, transactionTypeEntity.TransactionTypeName, transactionTypeEntity.Description).TransactionTypeDomain;

                return transactionTypeDomain;
            }
        }
        public TransactionTypeDomain Get(int idTransactionType)
        {
            using (var context = _factory())
            {
                var transactionTypeEntity = context.TransactionTypes.FirstOrDefault(x => x.IdTransactionType == idTransactionType);
                var transactionTypeDomain = TransactionTypeDomain.Create(transactionTypeEntity.IdTransactionType, transactionTypeEntity.TransactionTypeName, transactionTypeEntity.Description).TransactionTypeDomain;

                return transactionTypeDomain;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<TransactionTypeDomain> GetAsync(string transactionTypeName)
        {
            using (var context = _factory())
            {
                var transactionTypeEntity = await context.TransactionTypes.FirstOrDefaultAsync(x => x.TransactionTypeName == transactionTypeName);

                if (transactionTypeEntity == null) { return null; }

                var transactionTypeDomain = TransactionTypeDomain.Create(transactionTypeEntity.IdTransactionType, transactionTypeEntity.TransactionTypeName, transactionTypeEntity.Description).TransactionTypeDomain;

                return transactionTypeDomain;
            }
        }
        public TransactionTypeDomain Get(string transactionTypeName)
        {
            using (var context = _factory())
            {
                var transactionTypeEntity = context.TransactionTypes.FirstOrDefault(x => x.TransactionTypeName == transactionTypeName);

                if (transactionTypeEntity == null) { return null; }

                var transactionTypeDomain = TransactionTypeDomain.Create(transactionTypeEntity.IdTransactionType, transactionTypeEntity.TransactionTypeName, transactionTypeEntity.Description).TransactionTypeDomain;

                return transactionTypeDomain;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<int> UpdateAsync(int idTransactionType, string? transactionTypeName, string? description)
        {
            using (var context = _factory())
            {
                var entity = await context.TransactionTypes.FirstOrDefaultAsync(x => x.IdTransactionType == idTransactionType);

                entity.TransactionTypeName = transactionTypeName;
                entity.Description = description;

                context.TransactionTypes.Update(entity);
                context.SaveChanges();

                return idTransactionType;
            }
        }
        public int Update(int idTransactionType, string? transactionTypeName, string? description)
        {
            using (var context = _factory())
            {
                var entity = context.TransactionTypes.FirstOrDefault(x => x.IdTransactionType == idTransactionType);

                entity.TransactionTypeName = transactionTypeName;
                entity.Description = description;

                context.TransactionTypes.Update(entity);
                context.SaveChanges();

                return idTransactionType;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task DeleteAsync(int idTransactionType)
        {
            using (var context = _factory())
            {
                await context.TransactionTypes.Where(x => x.IdTransactionType == idTransactionType).ExecuteDeleteAsync();
            }
        }
        public void Delete(int idTransactionType)
        {
            using (var context = _factory())
            {
                context.TransactionTypes.Where(x => x.IdTransactionType == idTransactionType).ExecuteDelete();
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
