using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.DomainModels;
using MoneyFlow.Domain.Interfaces.Repositories;
using MoneyFlow.Infrastructure.Context;
using MoneyFlow.Infrastructure.EntityModel;

namespace MoneyFlow.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly Func<ContextMF> _factory;

        public UsersRepository(Func<ContextMF> factory)
        {
            _factory = factory;
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<int> CreateAsync(string userName, string login, string password)
        {
            using (var context = _factory())
            {
                var userEntity = new User
                {
                    UserName = userName,
                    Login = login,
                    Password = password
                };

                await context.AddAsync(userEntity);
                await context.SaveChangesAsync();

                return context.Users.FirstOrDefault(x => x.Login == login).IdUser;
            }
        }
        public int Create(string userName, string login, string password)
        {
            using (var context = _factory())
            {
                var userEntity = new User
                {
                    UserName = userName,
                    Login = login,
                    Password = password
                };

                context.Add(userEntity);
                context.SaveChanges();

                return context.Users.FirstOrDefault(x => x.Login == login).IdUser;
            }
        }

        public async Task CreateDefaultRecordAsync(int idUser)
        {
            using (var context = _factory())
            {
                await context.Categories.AddAsync(new Category
                {
                    CategoryName = "Отсутствует",
                    IdUser = idUser
                });
                await context.SaveChangesAsync();

                await context.Subcategories.AddAsync(new Subcategory
                {
                    SubcategoryName = "Отсутствует",
                    IdUser = idUser
                });
                await context.SaveChangesAsync();

                var category = await context.Categories.FirstOrDefaultAsync(x => x.IdUser == idUser);
                var subcategory = await context.Subcategories.FirstOrDefaultAsync(x => x.IdUser == idUser);

                var catLinkSub = await context.CatLinkSubs.AddAsync(new CatLinkSub
                {
                    IdCategory = category.IdCategory,
                    IdSubcategory = subcategory.IdSubcategory,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IdUser = idUser
                });

                await context.SaveChangesAsync();
            }
        }

        public void CreateDefaultRecord(int idUser)
        {
            Task.Run(() => CreateDefaultRecordAsync(idUser));
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<List<UserDomain>> GetAllAsync()
        {
            using (var context = _factory())
            {
                var userList = new List<UserDomain>();
                var userEntity = await context.Users.Include(x => x.IdGenderNavigation).ToListAsync();

                foreach (var item in userEntity)
                {
                    userList.Add(UserDomain.Create(item.IdUser, item.UserName, item.Avatar, item.Login, item.Password, item.IdGender).UserDomain);
                }

                return userList;
            }
        }
        public List<UserDomain> GetAll()
        {
            using (var context = _factory())
            {
                var userList = new List<UserDomain>();
                var userEntity = context.Users.Include(x => x.IdGenderNavigation).ToList();

                foreach (var item in userEntity)
                {
                    userList.Add(UserDomain.Create(item.IdUser, item.UserName, item.Avatar, item.Login, item.Password, item.IdGender).UserDomain);
                }

                return userList;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<UserDomain> GetAsync(int idUser)
        {
            using (var context = _factory())
            {
                var userEntity = await context.Users.Include(x => x.IdGenderNavigation).FirstOrDefaultAsync(x => x.IdUser == idUser);
                var userDomain = UserDomain.Create(userEntity.IdUser, userEntity.UserName, userEntity.Avatar, userEntity.Login, userEntity.Password, userEntity.IdGender).UserDomain;

                return userDomain;
            }
        }
        public UserDomain Get(int idUser)
        {
            using (var context = _factory())
            {
                var userEntity = context.Users.Include(x => x.IdGenderNavigation).FirstOrDefault(x => x.IdUser == idUser);
                var userDomain = UserDomain.Create(userEntity.IdUser, userEntity.UserName, userEntity.Avatar, userEntity.Login, userEntity.Password, userEntity.IdGender).UserDomain;

                return userDomain;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<UserDomain> GetAsync(string login)
        {
            using (var context = _factory())
            {
                var userEntity = await context.Users.Include(x => x.IdGenderNavigation).FirstOrDefaultAsync(x => x.Login == login);

                if (userEntity == null) { return null; }

                var userDomain = UserDomain.Create(userEntity.IdUser, userEntity.UserName, userEntity.Avatar, userEntity.Login, userEntity.Password, userEntity.IdGender).UserDomain;

                return userDomain;
            }
        }
        public UserDomain Get(string login)
        {
            using (var context = _factory())
            {
                var userEntity = context.Users.Include(x => x.IdGenderNavigation).FirstOrDefault(x => x.Login == login);

                if (userEntity == null) { return null; }

                var userDomain = UserDomain.Create(userEntity.IdUser, userEntity.UserName, userEntity.Avatar, userEntity.Login, userEntity.Password, userEntity.IdGender).UserDomain;

                return userDomain;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public UserTotalInfoDomain GetTotalUserInfo(int idUser)
        {
            using (var context = _factory())
            {
                var user = context.Users.FirstOrDefault(x => x.IdUser == idUser);

                if (string.IsNullOrEmpty(user.IdGenderNavigation?.GenderName))
                {
                    return UserTotalInfoDomain.Create("Отсутствует", 0, 0, 0, 0, 1, 1, 0).UserTotalInfoDomain;
                }

                var gender = context.Users.Include(x => x.IdGenderNavigation).FirstOrDefault(x => x.IdUser == idUser).IdGenderNavigation.GenderName;
                var totalBalance = context.Accounts.Where(x => x.IdUser == idUser).Sum(x => x.Balance);

                var accountCount = context.Accounts.Where(x => x.IdUser == idUser).Count();

                var accountId = context.Accounts.Where(x => x.IdUser == idUser).Select(x => x.IdAccountType).Distinct().ToList();
                var userAccountTypesCount = context.AccountTypes.Where(x => accountId.Contains(x.IdAccountType)).Count();

                var banksId = context.Accounts.Where(x => x.IdUser == idUser).Select(x => x.IdBank).Distinct().ToList();
                var bankCount = context.Banks.Where(x => banksId.Contains(x.IdBank)).Count();

                var catCount = context.Categories.Where(x => x.IdUser == idUser).Count();

                var subcatCount = context.CatLinkSubs.Where(x => x.IdUser == idUser).Select(x => x.IdSubcategory).Count();

                var financialRecordCount = context.FinancialRecords.Where(x => x.IdUser == idUser).Count();


                var domain = UserTotalInfoDomain.Create(gender, totalBalance, accountCount, userAccountTypesCount, bankCount, catCount, subcatCount, financialRecordCount).UserTotalInfoDomain;

                return domain;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<int> UpdateAsync(int idUser, string? userName, byte[]? avatar,
                                      string password, int? idGender)
        {
            using (var context = _factory())
            {
                var entity = await context.Users.FirstOrDefaultAsync(x => x.IdUser == idUser);

                entity.UserName = userName;
                entity.Avatar = avatar;
                entity.Password = password;
                entity.IdGender = idGender;

                context.Users.Update(entity);
                context.SaveChanges();

                return idUser;
            }
        }
        public int Update(int idUser, string? userName, byte[]? avatar,
                                      string password, int? idGender)
        {
            using (var context = _factory())
            {
                var entity = context.Users.FirstOrDefault(x => x.IdUser == idUser);

                entity.UserName = userName;
                entity.Avatar = avatar;
                entity.Password = password;
                entity.IdGender = idGender;

                context.Users.Update(entity);
                context.SaveChanges();

                return idUser;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task DeleteAsync(int idUser)
        {
            using (var context = _factory())
            {
                await context.Users.Where(x => x.IdUser == idUser).ExecuteDeleteAsync();
            }
        }
        public void Delete(int idUser)
        {
            using (var context = _factory())
            {
                context.Users.Where(x => x.IdUser == idUser).ExecuteDelete();
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
