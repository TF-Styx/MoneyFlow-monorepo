using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.DomainModels;
using MoneyFlow.Domain.Interfaces.Repositories;
using MoneyFlow.Infrastructure.Context;
using MoneyFlow.Infrastructure.EntityModel;

namespace MoneyFlow.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly Func<ContextMF> _factory;

        public AccountRepository(Func<ContextMF> factory)
        {
            _factory = factory;
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<int> CreateAsync(int? numberAccount, int idUser, BankDomain bankDomain, AccountTypeDomain accountTypeDomain, decimal? balance)
        {
            using (var context = _factory())
            {
                var accountEntity = new Account
                {
                    NumberAccount = numberAccount,
                    IdUser = idUser,
                    IdBank = bankDomain.IdBank,
                    IdAccountType = accountTypeDomain.IdAccountType,
                    Balance = balance
                };

                await context.AddAsync(accountEntity);
                await context.SaveChangesAsync();

                return context.Accounts.Where(x => x.IdUser == idUser)
                                       .OrderByDescending(x => x.IdAccount)
                                       .FirstOrDefault(x => x.NumberAccount == numberAccount).IdAccount;
            }
        }
        public int Create(int? numberAccount, int idUser, BankDomain bankDomain, AccountTypeDomain accountTypeDomain, decimal? balance)
        {
            using (var context = _factory())
            {
                var accountEntity = new Account
                {
                    NumberAccount = numberAccount,
                    IdUser = idUser,
                    IdBank = bankDomain.IdBank,
                    IdAccountType = accountTypeDomain.IdAccountType,
                    Balance = balance
                };

                context.Add(accountEntity);
                context.SaveChanges();

                return context.Accounts.Where(x => x.IdUser == idUser)
                                        .OrderByDescending(x => x.IdAccount)
                                        .FirstOrDefault(x => x.NumberAccount == numberAccount).IdAccount;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<List<AccountDomain>> GetAllAsync(int idUser)
        {
            using (var context = _factory())
            {
                var accountList = new List<AccountDomain>();
                var banks = await context.Banks.ToListAsync();
                var accountsType = await context.AccountTypes.ToListAsync();
                var accountEntity = await context.Accounts.Where(x => x.IdUser == idUser).ToListAsync();

                foreach (var item in accountEntity)
                {
                    var bank = banks.FirstOrDefault(x => x.IdBank == item.IdBank);
                    var accountType = accountsType.FirstOrDefault(x => x.IdAccountType == item.IdAccountType);

                    accountList.Add(AccountDomain.Create(item.IdAccount, item.NumberAccount,
                                    BankDomain.Create(bank.IdBank, bank.BankName).BankDomain,
                                    AccountTypeDomain.Create(accountType.IdAccountType, accountType.AccountTypeName).AccountTypeDomain,
                                    item.Balance).AccountDomain);
                }

                return accountList;
            }
        }
        public List<AccountDomain> GetAll(int idUser)
        {
            using (var context = _factory())
            {
                var accountList = new List<AccountDomain>();
                var banks = context.Banks.ToList();
                var accountsType = context.AccountTypes.ToList();
                var accountEntity = context.Accounts.Where(x => x.IdUser == idUser).ToList();

                foreach (var item in accountEntity)
                {
                    var bank = banks.FirstOrDefault(x => x.IdBank == item.IdBank);
                    var accountType = accountsType.FirstOrDefault(x => x.IdAccountType == item.IdAccountType);

                    accountList.Add(AccountDomain.Create(item.IdAccount, item.NumberAccount,
                                    BankDomain.Create(bank.IdBank, bank.BankName).BankDomain,
                                    AccountTypeDomain.Create(accountType.IdAccountType, accountType.AccountTypeName).AccountTypeDomain,
                                    item.Balance).AccountDomain);
                }

                return accountList;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<AccountDomain> GetAsync(int idAccount)
        {
            using (var context = _factory())
            {
                var accountEntity = await context.Accounts.FirstOrDefaultAsync(x => x.IdAccount == idAccount);
                var bank = await context.Banks.FirstOrDefaultAsync(x => x.IdBank == accountEntity.IdBank);
                var accountType = await context.AccountTypes.FirstOrDefaultAsync(x => x.IdAccountType == accountEntity.IdAccountType);
                var accountDomain = AccountDomain.Create(accountEntity.IdAccount, accountEntity.NumberAccount, 
                                    BankDomain.Create(bank.IdBank, bank.BankName).BankDomain,
                                    AccountTypeDomain.Create(accountType.IdAccountType, accountType.AccountTypeName).AccountTypeDomain, 
                                    accountEntity.Balance).AccountDomain;

                return accountDomain;
            }
        }
        public AccountDomain Get(int idAccount)
        {
            using (var context = _factory())
            {
                var accountEntity = context.Accounts.FirstOrDefault(x => x.IdAccount == idAccount);
                var bank = context.Banks.FirstOrDefault(x => x.IdBank == accountEntity.IdBank);
                var accountType = context.AccountTypes.FirstOrDefault(x => x.IdAccountType == accountEntity.IdAccountType);
                var accountDomain = AccountDomain.Create(accountEntity.IdAccount, accountEntity.NumberAccount,
                                    BankDomain.Create(bank.IdBank, bank.BankName).BankDomain,
                                    AccountTypeDomain.Create(accountType.IdAccountType, accountType.AccountTypeName).AccountTypeDomain,
                                    accountEntity.Balance).AccountDomain;

                return accountDomain;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<AccountDomain> GetAsync(int? numberAccount)
        {
            using (var context = _factory())
            {
                var accountEntity = await context.Accounts.FirstOrDefaultAsync(x => x.NumberAccount == numberAccount);
                var bank = await context.Banks.FirstOrDefaultAsync(x => x.IdBank == accountEntity.IdBank);
                var accountType = await context.AccountTypes.FirstOrDefaultAsync(x => x.IdAccountType == accountEntity.IdAccountType);
                var accountDomain = AccountDomain.Create(accountEntity.IdAccount, accountEntity.NumberAccount,
                                    BankDomain.Create(bank.IdBank, bank.BankName).BankDomain,
                                    AccountTypeDomain.Create(accountType.IdAccountType, accountType.AccountTypeName).AccountTypeDomain,
                                    accountEntity.Balance).AccountDomain;

                return accountDomain;
            }
        }
        public AccountDomain Get(int? numberAccount)
        {
            using (var context = _factory())
            {
                var accountEntity = context.Accounts.FirstOrDefault(x => x.NumberAccount == numberAccount);
                var bank = context.Banks.FirstOrDefault(x => x.IdBank == accountEntity.IdBank);
                var accountType = context.AccountTypes.FirstOrDefault(x => x.IdAccountType == accountEntity.IdAccountType);
                var accountDomain = AccountDomain.Create(accountEntity.IdAccount, accountEntity.NumberAccount,
                                    BankDomain.Create(bank.IdBank, bank.BankName).BankDomain,
                                    AccountTypeDomain.Create(accountType.IdAccountType, accountType.AccountTypeName).AccountTypeDomain,
                                    accountEntity.Balance).AccountDomain;

                return accountDomain;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<int> UpdateAsync(int idAccount, int? numberAccount, BankDomain bankDomain, AccountTypeDomain accountTypeDomain, decimal? balance)
        {
            using (var context = _factory())
            {
                var entity = await context.Accounts.FirstOrDefaultAsync(x => x.IdAccount == idAccount);

                entity.NumberAccount = numberAccount;
                entity.IdBank = bankDomain.IdBank;
                entity.IdAccountType = accountTypeDomain.IdAccountType;
                entity.Balance = balance;

                context.Accounts.Update(entity);
                context.SaveChanges();

                return idAccount;
            }
        }
        public int Update(int idAccount, int? numberAccount, BankDomain bankDomain, AccountTypeDomain accountTypeDomain, decimal? balance)
        {
            using (var context = _factory())
            {
                var entity = context.Accounts.FirstOrDefault(x => x.IdAccount == idAccount);

                entity.NumberAccount = numberAccount;
                entity.IdBank = bankDomain.IdBank;
                entity.IdAccountType = accountTypeDomain.IdAccountType;
                entity.Balance = balance;

                context.Accounts.Update(entity);
                context.SaveChanges();

                return idAccount;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task DeleteAsync(int idAccounts)
        {
            using (var context = _factory())
            {
                await context.Accounts.Where(x => x.IdAccount == idAccounts).ExecuteDeleteAsync();
            }
        }
        public void Delete(int idAccounts)
        {
            using (var context = _factory())
            {
                context.Accounts.Where(x => x.IdAccount == idAccounts).ExecuteDeleteAsync();
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
