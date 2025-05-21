using MoneyFlow.AuthenticationService.Domain.DomainModels;
using System.Runtime.CompilerServices;

namespace MoneyFlow.AuthenticationService.Application.InterfaceRepositories
{
    public interface IUserRepository
    {
        Task<UserDomain> CreateAsync(UserDomain userDomain);

        /// <summary>
        /// Возвращает хранимый хэш пароля или исключение
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task<string> GetHashByLoginAsync(string login);
        Task<UserDomain?> GetUserByLoginAsync(string login);

        Task UpdateDataEntryAsync(int idUser);
        Task<bool> UpdatePasswordAsync(string email, string login, string newHash);

        Task<bool> ExistByIdUserAsync(int idUser);
        Task<bool> ExistByLoginAsync(string login);
        Task<bool> ExistByEmailAsync(string email);
        Task<bool> ExistByPhoneAsync(string? phone);
    }
}