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
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Login == entity.Login && x.UserName == entity.UserName);

            Login.TryCreate(entity.Login, out var login);
            EmailAddress.TryCreate(entity.Email, out var email);
            PhoneNumber.TryCreate(entity.Phone, out var phone);

            var (User, Message) = UserDomain.Create(entity.IdUser, login, entity.UserName, entity.PasswordHash, email, phone, entity.IdGender, entity.IdRole);

            return User;
        }

        //public async IAsyncEnumerable<UserDomain> GetAllStreamingAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        //{
        //    var entitiesStream = _context.Users.AsNoTracking().AsAsyncEnumerable().WithCancellation(cancellationToken);

        //    await foreach (var entity in entitiesStream)
        //    {
        //        var (User, Message) = UserDomain.Create(entity.IdUser, entity.Login, entity.UserName, entity.PasswordHash, entity.Salt, entity.Email, entity.Phone, entity.DateRegistration, entity.DateEntry, entity.DateUpdate, entity.IdGender, entity.IdRole);

        //        if (User is not null)
        //            yield return User;
        //    }
        //}

        //public async Task<UserDomain?> GetByIdAsync(int idUser)
        //{
        //    var entity = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.IdUser == idUser);

        //    var (User, Message) = UserDomain.Create(entity.IdUser, entity.Login, entity.UserName, entity.PasswordHash, entity.Salt, entity.Email, entity.Phone, entity.DateRegistration, entity.DateEntry, entity.DateUpdate, entity.IdGender, entity.IdRole);

        //    return User;
        //}

        //public async Task<UserDomain?> UpdateAsync(UserDomain userDomain)
        //{
        //    var entity = await _context.Users.FirstOrDefaultAsync(x => x.IdUser == userDomain.IdUser);

        //    entity.UserName = userDomain.UserName;
        //    entity.PasswordHash = userDomain.PasswordHash;
        //    entity.Salt = userDomain.Salt;
        //    entity.Email = userDomain.Email;
        //    entity.Phone = userDomain.Phone;
        //    entity.DateUpdate = userDomain.DateUpdate;
        //    entity.IdGender = userDomain.IdGender;
        //    entity.IdRole = userDomain.IdRole;

        //    var (User, Message) = UserDomain.Create(entity.IdUser, entity.Login, entity.UserName, entity.PasswordHash, entity.Salt, entity.Email, entity.Phone, entity.DateRegistration, entity.DateEntry, entity.DateUpdate, entity.IdGender, entity.IdRole);

        //    await _context.SaveChangesAsync();

        //    return User;
        //}

        //public async Task<int> DeleteAsync(int idUser)
        //{
        //    await _context.Users.Where(x => x.IdUser == idUser).ExecuteDeleteAsync();

        //    await _context.SaveChangesAsync();

        //    return idUser;
        //}


        public async Task<bool> ExistByIdUserAsync(int idUser)  => await _context.Users.AnyAsync(x => x.IdUser == idUser);

        public async Task<bool> ExistByLoginAsync(string login) => await _context.Users.AnyAsync(x => x.Login.ToLower() == login.ToLower());

        public async Task<bool> ExistByEmailAsync(string email) => await _context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());

        public async Task<bool> ExistByPhoneAsync(string? phone)
        {
            if (phone is null)
                return false;

            return await _context.Users.AnyAsync(x => x.Phone == phone);
        }
    }
}
