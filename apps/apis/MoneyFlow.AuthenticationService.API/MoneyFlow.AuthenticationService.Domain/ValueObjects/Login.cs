using MoneyFlow.AuthenticationService.Domain.Constants;
using MoneyFlow.AuthenticationService.Domain.DomainModels.Validations;
using System.Text.RegularExpressions;

namespace MoneyFlow.AuthenticationService.Domain.ValueObjects
{
    public class Login : IEquatable<Login>
    {
        private Login(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public static ResultValidation TryCreate(string? loginUser, out Login? login)
        {
            login = null;

            var errorList = new List<string>();

            if (string.IsNullOrWhiteSpace(loginUser))
            {
                errorList.Add("Не заполнено поле 'Login'!!");
                return ResultValidation.Failure(errorList);
            }

            if (loginUser.Length >= UserConstants.MaxLoginLength)
            {
                errorList.Add($"Длина поля 'Login' превышает допустимое значение в '{UserConstants.MaxLoginLength}' символов!!");
                return ResultValidation.Failure(errorList);
            }
            if (loginUser.Length <= UserConstants.MinLoginLength)
            {
                errorList.Add($"Длины поля 'Login' не может быть меньше '{UserConstants.MinLoginLength}' символов!!");
                return ResultValidation.Failure(errorList);
            }

            if (!Regex.IsMatch(loginUser, @"^[a-zA-Z]+$"))
            {
                errorList.Add("Неверный формат логина. Допускаться только английские буквы любого регистра.");
            }

            if (errorList.Any())
                return ResultValidation.Failure(errorList);

            login = new Login(loginUser.ToLowerInvariant());

            return ResultValidation.Success();
        }

        public bool Equals(Login? other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != this.GetType())
                return false;

            return Equals((Login)obj);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value;

        public static bool operator ==(Login? left, Login? right) => Equals(left, right);

        public static bool operator !=(Login? left, Login? right) => !Equals(left, right);
    }
}
