using MoneyFlow.AuthenticationService.Domain.Constants;
using MoneyFlow.AuthenticationService.Domain.ValueObjects;
using System.Diagnostics;

namespace MoneyFlow.AuthenticationService.Domain.DomainModels
{
    public class UserDomain
    {
        private UserDomain(int idUser, string login, string userName, string passwordHash, string email, string? phone, DateTime dateRegistration, DateTime dateEntry, DateTime dateUpdate, int? idGender, int idRole)
        {
            IdUser = idUser;
            Login = login;
            UserName = userName;
            PasswordHash = passwordHash;
            Email = email;
            Phone = phone;
            DateRegistration = dateRegistration;
            DateEntry = dateEntry;
            DateUpdate = dateUpdate;
            IdGender = idGender;
            IdRole = idRole;
        }

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

        public static (UserDomain? UserDomain, string Message) Create(int idUser, Login? login, string userName, string passwordHash, EmailAddress? email, PhoneNumber? phone, int? idGender, int idRole)
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

            #endregion

            var dateRegistration = DateTime.UtcNow;
            var dateEntry = DateTime.UtcNow;
            var dateUpdate = DateTime.UtcNow;

            var domain = new UserDomain(idUser, login.Value, userName, passwordHash, email.Value, phone.Value, dateRegistration, dateEntry, dateUpdate, idGender, idRole);

            return (domain, message);
        }
    }
}
