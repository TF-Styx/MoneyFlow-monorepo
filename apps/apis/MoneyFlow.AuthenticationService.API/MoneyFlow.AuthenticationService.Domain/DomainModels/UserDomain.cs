using MoneyFlow.AuthenticationService.Domain.Constants;
using MoneyFlow.AuthenticationService.Domain.ValueObjects;
using System.Diagnostics;

namespace MoneyFlow.AuthenticationService.Domain.DomainModels
{
    public class UserDomain
    {
        private UserDomain() { }

        public int IdUser { get; private set; }
        public string Login { get; private set; } = null!;
        public string UserName { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string? Phone { get; private set; }
        public DateTime DateRegistration { get; private set; }
        public DateTime DateEntry { get; private set; }
        public DateTime DateUpdate { get; private set; }
        public int? IdGender { get; private set; }
        public int IdRole { get; private set; }

        public static (UserDomain? UserDomain, string Message) Create(Login? login, string userName, string passwordHash, EmailAddress? email, PhoneNumber? phone, int? idGender, int idRole)
        {
            string message = string.Empty;

            #region Проверки

            if (login is null)
            {
                message = "Не прошла валидация поле 'Login' через ValueObject";
                return (null, message);
            }
            if (email is null)
            {
                message = "Не прошел валидацию поле 'Email' через ValueObject";
                return (null, message);
            }
            if (phone is null)
            {
                Debug.WriteLine($"Не прошел валидацию поле 'Email' через ValueObject. У пользователя {login}");
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                message =
                    $"Вы не заполнили поле 'User Name'!!\n" +
                    $"Максимальная допустимая длина данного поля '{UserConstants.MaxUserNameLength}' символов!!\n" +
                    $"Минимальная допустимая длина данного поля '{UserConstants.MinUserNameLength}' символов!!";
                return (null, message);
            }
            if (userName.Length >= UserConstants.MaxUserNameLength)
            {
                message = $"Длина поля 'User Name' превышает допустимое значение в '{UserConstants.MaxUserNameLength}' символов!!";
                return (null, message);
            }
            if (userName.Length <= UserConstants.MinUserNameLength)
            {
                message = $"Длины поля 'User Name' не может быть меньше '{UserConstants.MinUserNameLength}' символов!!";
                return (null, message);
            }

            int? idGenderDefault = null;

            if (!idGender.HasValue)
                idGenderDefault = 1;
            else
                idGenderDefault = idGender.Value;

            #endregion

            var dateRegistration = DateTime.UtcNow;
            var dateEntry = DateTime.UtcNow;
            var dateUpdate = DateTime.UtcNow;

            var domain = new UserDomain()
            {
                Login = login.Value,
                UserName = userName,
                PasswordHash = passwordHash,
                Email = email.Value,
                Phone = phone?.Value,
                IdGender = idGenderDefault,
                IdRole = idRole,
                DateRegistration = dateRegistration,
                DateEntry = dateEntry,
                DateUpdate = dateUpdate
            };
            return (domain, message);
        }

        public static UserDomain Reconstitute(int idUser, string login, string userName, 
                                              string passwordHash, string email, string? phone, 
                                              DateTime dateRegistration, DateTime dateEntry, DateTime dateUpdate, 
                                              int? idGender, int idRole)
        {
            return new UserDomain()
            {
                IdUser = idUser,
                Login = login,
                UserName = userName,
                PasswordHash = passwordHash,
                Email = email,
                Phone = phone,
                DateRegistration = dateRegistration,
                DateEntry = dateEntry,
                DateUpdate = dateUpdate,
                IdGender = idGender,
                IdRole = idRole
            };
        }
    }
}
