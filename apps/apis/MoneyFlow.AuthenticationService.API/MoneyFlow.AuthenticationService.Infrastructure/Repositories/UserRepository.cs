using Microsoft.EntityFrameworkCore;
using MoneyFlow.AuthenticationService.Application.InterfaceRepositories;
using MoneyFlow.AuthenticationService.Domain.DomainModels;
using MoneyFlow.AuthenticationService.Domain.ValueObjects;
using MoneyFlow.AuthenticationService.Infrastructure.Data;
using MoneyFlow.AuthenticationService.Infrastructure.Data.Entities;

namespace MoneyFlow.AuthenticationService.Infrastructure.Repositories
{
    public class UserRepository(Context context) : IUserRepository
    {
        private readonly Context _context = context;

        #region Create

        public async Task<UserDomain> CreateAsync(UserDomain userDomain)
        {
            var entity = new User
            {
                Login = userDomain.Login,
                UserName = userDomain.UserName,
                PasswordHash = userDomain.PasswordHash,
                Email = userDomain.Email,
                Phone = userDomain.Phone,
                DateRegistration = userDomain.DateRegistration,
                DateEntry = userDomain.DateEntry,
                DateUpdate = userDomain.DateUpdate,
                IdGender = userDomain.IdGender,
                IdRole = userDomain.IdRole,
            };

            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();

            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Login == entity.Login && x.UserName == entity.UserName);

            var domain = UserDomain.Reconstitute(user.IdUser, user.Login, user.UserName, user.PasswordHash, user.Email, user.Phone, user.DateRegistration, user.DateEntry, user.DateUpdate, user.IdGender, user.IdRole);

            return domain;
        }

        #endregion


        #region Get

        public async Task<string> GetHashByLoginAsync(string login)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Login.ToLower() == login.ToLower());

            return user is null ? throw new ArgumentNullException("Пользователь не найден!!") : user.PasswordHash;
        }

        public async Task<UserDomain?> GetUserByLoginAsync(string login)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Login.ToLower() == login.ToLower());

            if (user is null)
                return null;

            return UserDomain.Reconstitute(user.IdUser, user.Login, user.UserName, user.PasswordHash, user.Email, user.Phone, user.DateRegistration, user.DateEntry, user.DateUpdate, user.IdGender, user.IdRole);
        }

        #endregion


        #region Update

        public async Task<bool> UpdatePasswordAsync(string email, string login, string newHash)
        {
            var updatedRows = await _context.Users
                .Where(x => x.Login.ToLower() == login.ToLower() && x.Email.ToLower() == email.ToLower())
                    .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.PasswordHash, newHash));

            if (updatedRows > 0)
                return true;
            else
                return false;
        }

        #endregion


        #region Exist

        public async Task<bool> ExistByIdUserAsync(int idUser)  => await _context.Users.AsNoTracking().AnyAsync(x => x.IdUser == idUser);

        public async Task<bool> ExistByLoginAsync(string login) => await _context.Users.AsNoTracking().AnyAsync(x => x.Login.ToLower() == login.ToLower());

        public async Task<bool> ExistByEmailAsync(string email) => await _context.Users.AsNoTracking().AnyAsync(x => x.Email.ToLower() == email.ToLower());

        public async Task<bool> ExistByPhoneAsync(string? phone)
        {
            if (phone is null)
                return false;

            return await _context.Users.AnyAsync(x => x.Phone == phone);
        }

        #endregion
    }
}
