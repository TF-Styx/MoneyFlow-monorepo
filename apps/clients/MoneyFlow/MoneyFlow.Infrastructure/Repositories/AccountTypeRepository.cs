using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.DomainModels;
using MoneyFlow.Domain.Interfaces.Repositories;
using MoneyFlow.Infrastructure.Context;
using MoneyFlow.Infrastructure.EntityModel;

namespace MoneyFlow.Infrastructure.Repositories
{
    public class AccountTypeRepository : IAccountTypeRepository
    {
        private readonly Func<ContextMF> _factory;

        public AccountTypeRepository(Func<ContextMF> factory)
        {
            _factory = factory;
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<int> CreateAsync(string accountTypeName)
        {
            using (var context = _factory())
            {
                var accountTypeEntity = new AccountType
                {
                    AccountTypeName = accountTypeName,
                };

                await context.AddAsync(accountTypeEntity);
                await context.SaveChangesAsync();

                return context.AccountTypes.FirstOrDefault(x => x.AccountTypeName == accountTypeName).IdAccountType;
            }
        }
        public int Create(string accountTypeName)
        {
            using (var context = _factory())
            {
                var accountTypeEntity = new AccountType
                {
                    AccountTypeName = accountTypeName
                };

                context.AddAsync(accountTypeEntity);
                context.SaveChangesAsync();

                return context.AccountTypes.FirstOrDefault(x => x.AccountTypeName == accountTypeName).IdAccountType;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<List<AccountTypeDomain>> GetAllAsync()
        {
            using (var context = _factory())
            {
                var accountTypeList = new List<AccountTypeDomain>();
                var accountTypeEntities = await context.AccountTypes.ToListAsync();

                foreach (var item in accountTypeEntities)
                {
                    accountTypeList.Add(AccountTypeDomain.Create(item.IdAccountType, item.AccountTypeName).AccountTypeDomain);
                }

                return accountTypeList;
            }
        }
        public List<AccountTypeDomain> GetAll()
        {
            using (var context = _factory())
            {
                var accountTypeList = new List<AccountTypeDomain>();
                var accountTypeEntities = context.AccountTypes.ToList();

                foreach (var item in accountTypeEntities)
                {
                    accountTypeList.Add(AccountTypeDomain.Create(item.IdAccountType, item.AccountTypeName).AccountTypeDomain);
                }

                return accountTypeList;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<AccountTypeDomain> GetAsync(int idAccountType)
        {
            using (var context = _factory())
            {
                var accountTypeEntity = await context.AccountTypes.FirstOrDefaultAsync(x => x.IdAccountType == idAccountType);
                var accountTypeDomain = AccountTypeDomain.Create(idAccountType, accountTypeEntity.AccountTypeName).AccountTypeDomain;

                return accountTypeDomain;
            }
        }
        public AccountTypeDomain Get(int idAccountType)
        {
            using (var context = _factory())
            {
                var accountTypeEntity = context.AccountTypes.FirstOrDefault(x => x.IdAccountType == idAccountType);
                var accountTypeDomain = AccountTypeDomain.Create(idAccountType, accountTypeEntity.AccountTypeName).AccountTypeDomain;

                return accountTypeDomain;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<AccountTypeDomain> GetAsync(string accountTypeName)
        {
            using (var context = _factory())
            {
                var accountTypeEntity = await context.AccountTypes.FirstOrDefaultAsync(x => x.AccountTypeName == accountTypeName);

                if (accountTypeEntity == null) { return null; }

                var accountTypeDomain = AccountTypeDomain.Create(accountTypeEntity.IdAccountType, accountTypeName).AccountTypeDomain;

                return accountTypeDomain;
            }
        }
        public AccountTypeDomain Get(string accountTypeName)
        {
            using (var context = _factory())
            {
                var accountTypeEntity = context.AccountTypes.FirstOrDefault(x => x.AccountTypeName == accountTypeName);

                if (accountTypeEntity == null) { return null; }

                var accountTypeDomain = AccountTypeDomain.Create(accountTypeEntity.IdAccountType, accountTypeName).AccountTypeDomain;

                return accountTypeDomain;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<UserAccountTypesDomain> GetByIdUserAsync(int idUser)
        {
            using (var context = _factory())
            {
                var accountId = await context.Accounts.Where(x => x.IdUser == idUser).Select(x => x.IdAccountType).Distinct().ToListAsync();
                var userAccountTypes = await context.AccountTypes.Where(x => accountId.Contains(x.IdAccountType)).ToListAsync();
                var accountTypeDomain = new List<AccountTypeDomain>();

                foreach (var item in userAccountTypes)
                {
                    accountTypeDomain.Add(AccountTypeDomain.Create(item.IdAccountType, item.AccountTypeName).AccountTypeDomain);
                }

                var domain = UserAccountTypesDomain.Create(idUser, accountTypeDomain).UserAccountTypesDomain;

                return domain;
            }
        }
        public UserAccountTypesDomain GetByIdUser(int idUser)
        {
            using (var context = _factory())
            {
                var accountId = context.Accounts.Where(x => x.IdUser == idUser).Select(x => x.IdAccountType).Distinct().ToList();
                var userAccountTypes = context.AccountTypes.Where(x => accountId.Contains(x.IdAccountType)).ToList();
                var accountTypeDomain = new List<AccountTypeDomain>();

                foreach (var item in userAccountTypes)
                {
                    accountTypeDomain.Add(AccountTypeDomain.Create(item.IdAccountType, item.AccountTypeName).AccountTypeDomain);
                }

                var domain = UserAccountTypesDomain.Create(idUser, accountTypeDomain).UserAccountTypesDomain;

                return domain;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<int> UpdateAsync(int idAccountType, string accountTypeName)
        {
            using (var context = _factory())
            {
                var entity = await context.AccountTypes.FirstOrDefaultAsync(x => x.IdAccountType == idAccountType);
                entity.AccountTypeName = accountTypeName;

                context.AccountTypes.Update(entity);
                context.SaveChanges();

                return idAccountType;
            }
        }
        public int Update(int idAccountType, string accountTypeName)
        {
            using (var context = _factory())
            {
                var entity = context.AccountTypes.FirstOrDefault(x => x.IdAccountType == idAccountType);
                entity.AccountTypeName = accountTypeName;

                context.AccountTypes.Update(entity);
                context.SaveChanges();

                return idAccountType;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task DeleteAsync(int idAccountType)
        {
            using (var context = _factory())
            {
                await context.AccountTypes.Where(x => x.IdAccountType == idAccountType).ExecuteDeleteAsync();
            }
        }
        public void Delete(int idAccountType)
        {
            using (var context = _factory())
            {
                context.AccountTypes.Where(x => x.IdAccountType == idAccountType).ExecuteDeleteAsync();
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
