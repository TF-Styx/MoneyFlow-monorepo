using MoneyFlow.AuthenticationService.Domain.DomainModels.Validations;

namespace MoneyFlow.AuthenticationService.Domain.ValueObjects
{
    public class Password : IEquatable<Password>
    {
        public const int MinPasswordLength = 10;

        private Password(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public static ResultValidation TryCreate(string? passwordUser, out Password? password)
        {
            password = null;

            var errorList = new List<string>();

            if (string.IsNullOrWhiteSpace(passwordUser))
            {
                errorList.Add("Не заполнено поле 'Password'!!");
                return ResultValidation.Failure(errorList);
            }

            if (!passwordUser.Any(char.IsUpper))
                errorList.Add("В пароле должна быть хоть одна буква верхнего регистра!!");

            if (!passwordUser.Any(char.IsLower))
                errorList.Add("В пароле должна быть хоть одна буква нижнего регистра!!");

            if (!passwordUser.Any(char.IsDigit))
                errorList.Add("В пароле должна быть хоть одна цифра!!");

            if (!passwordUser.Any(x => char.IsPunctuation(x) || char.IsSymbol(x)))
                errorList.Add("В пароле должна быть хоть один символ!!");

            if (passwordUser.Length < MinPasswordLength)
                errorList.Add($"Длина пароля не должна быть меньше {MinPasswordLength}");

            if (errorList.Any())
                return ResultValidation.Failure(errorList);

            password = new Password(passwordUser.ToLowerInvariant());

            return ResultValidation.Success();
        }

        public bool Equals(Password? other)
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

            return Equals((Password)obj);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value;

        public static bool operator ==(Password? left, Password? right) => Equals(left, right);

        public static bool operator !=(Password? left, Password? right) => !Equals(left, right);
    }
}
