using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.DomainModels;
using MoneyFlow.Domain.Interfaces.Repositories;
using MoneyFlow.Infrastructure.Context;
using MoneyFlow.Infrastructure.EntityModel;

namespace MoneyFlow.Infrastructure.Repositories
{
    public class BanksRepository : IBanksRepository
    {
        private readonly Func<ContextMF> _factory;

        public BanksRepository(Func<ContextMF> factory)
        {
            _factory = factory;
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<int> CreateAsync(string bankName)
        {
            using (var context = _factory())
            {
                var bankEntity = new Bank
                {
                    BankName = bankName
                };

                await context.AddAsync(bankEntity);
                await context.SaveChangesAsync();

                return context.Banks.FirstOrDefault(x => x.BankName == bankName).IdBank;
            }
        }
        public int Create(string bankName)
        {
            using (var context = _factory())
            {
                var bankEntity = new Bank
                {
                    BankName = bankName
                };

                context.AddAsync(bankEntity);
                context.SaveChangesAsync();

                return context.Banks.FirstOrDefault(x => x.BankName == bankName).IdBank;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<List<BankDomain>> GetAllAsync()
        {
            using (var context = _factory())
            {
                var bankList = new List<BankDomain>();
                var bankEntities = await context.Banks.ToListAsync();

                foreach (var item in bankEntities)
                {
                    bankList.Add(BankDomain.Create(item.IdBank, item.BankName).BankDomain);
                }

                return bankList;
            }
        }
        public List<BankDomain> GetAll()
        {
            using (var context = _factory())
            {
                var bankList = new List<BankDomain>();
                var bankEntities = context.Banks.ToList();

                foreach (var item in bankEntities)
                {
                    bankList.Add(BankDomain.Create(item.IdBank, item.BankName).BankDomain);
                }

                return bankList;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<BankDomain> GetAsync(int idBank)
        {
            using (var context = _factory())
            {
                var bankEntity = await context.Banks.FirstOrDefaultAsync(x => x.IdBank == idBank);
                var bankDomain = BankDomain.Create(idBank, bankEntity.BankName).BankDomain;

                return bankDomain;
            }
        }
        public BankDomain Get(int idBank)
        {
            using (var context = _factory())
            {
                var bankEntity = context.Banks.FirstOrDefault(x => x.IdBank == idBank);
                var bankDomain = BankDomain.Create(idBank, bankEntity.BankName).BankDomain;

                return bankDomain;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<BankDomain> GetAsync(string bankName)
        {
            using (var context = _factory())
            {
                var bankEntity = await context.Banks.FirstOrDefaultAsync(x => x.BankName == bankName);

                if (bankEntity == null) { return null; }

                var bankDomain = BankDomain.Create(bankEntity.IdBank, bankName).BankDomain;

                return bankDomain;
            }
        }
        public BankDomain Get(string bankName)
        {
            using (var context = _factory())
            {
                var bankEntity = context.Banks.FirstOrDefault(x => x.BankName == bankName);

                if (bankEntity == null) { return null; }

                var bankDomain = BankDomain.Create(bankEntity.IdBank, bankName).BankDomain;

                return bankDomain;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<UserBanksDomain> GetByIdUserAsync(int idUser)
        {
            using (var context = _factory())
            {
                var banksId = await context.Accounts.Where(x => x.IdUser == idUser).Select(x => x.IdBank).Distinct().ToListAsync();
                var userBanks = await context.Banks.Where(x => banksId.Contains(x.IdBank)).ToListAsync();
                var bankDomain = new List<BankDomain>();

                foreach (var item in userBanks)
                {
                    bankDomain.Add(BankDomain.Create(item.IdBank, item.BankName).BankDomain);
                }

                var domain = UserBanksDomain.Create(idUser, bankDomain).UserBanksDomain;

                return domain;
            }
        }        
        public UserBanksDomain GetByIdUser(int idUser)
        {
            using (var context = _factory())
            {
                var banksId = context.Accounts.Where(x => x.IdUser == idUser).Select(x => x.IdBank).Distinct().ToList();
                var userBanks = context.Banks.Where(x => banksId.Contains(x.IdBank)).ToList();
                var bankDomain = new List<BankDomain>();

                foreach (var item in userBanks)
                {
                    bankDomain.Add(BankDomain.Create(item.IdBank, item.BankName).BankDomain);
                }

                var domain = UserBanksDomain.Create(idUser, bankDomain).UserBanksDomain;

                return domain;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<int> UpdateAsync(int idBank, string bankName)
        {
            using (var context = _factory())
            {
                var entity = await context.Banks.FirstOrDefaultAsync(x => x.IdBank == idBank);
                entity.BankName = bankName;

                context.Banks.Update(entity);
                context.SaveChanges();

                return idBank;
            }
        }
        public int Update(int idBank, string bankName)
        {
            using (var context = _factory())
            {
                var entity = context.Banks.FirstOrDefault(x => x.IdBank == idBank);
                entity.BankName = bankName;

                context.Banks.Update(entity);
                context.SaveChanges();

                return idBank;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task DeleteAsync(int idBank)
        {
            using (var context = _factory())
            {
                await context.Banks.Where(x => x.IdBank == idBank).ExecuteDeleteAsync();
            }
        }
        public void Delete(int idBank)
        {
            using (var context = _factory())
            {
                context.Banks.Where(x => x.IdBank == idBank).ExecuteDelete();
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
