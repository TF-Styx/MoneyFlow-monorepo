using MoneyFlow.AuthenticationService.Domain.Constants;
using MoneyFlow.AuthenticationService.Domain.DomainModels.Validations;
using System.Text.RegularExpressions;

namespace MoneyFlow.AuthenticationService.Domain.ValueObjects
{
    public class PhoneNumber : IEquatable<PhoneNumber>
    {
        private PhoneNumber(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public static ResultValidation TryCreate(string? phone, out PhoneNumber? phoneNumber)
        {
            phoneNumber = null;

            var errorList = new List<string>();

            if (string.IsNullOrWhiteSpace(phone))
                return ResultValidation.Success();

            string normalizedPhone = Regex.Replace(phone, @"[^\d+]", "");

            if (phone.Trim().StartsWith("+"))
                normalizedPhone = "+" + Regex.Replace(phone, @"[^\d]", "");
            else
                normalizedPhone = Regex.Replace(phone, @"[^\d]", "");

            if (!Regex.IsMatch(normalizedPhone, @"^\+?\d{7,15}$"))
                errorList.Add("Не верный формат номера телефона!!\nОт 7 до 15 цифр, возможно с '+' вначале!!");

            if (normalizedPhone.Length >= UserConstants.MaxPhoneLength)
            {
                errorList.Add($"Длина поля 'Phone' превышает допустимое значение в '{UserConstants.MaxPhoneLength}' символов!!");
                return ResultValidation.Failure(errorList);
            }
            if (normalizedPhone.Length <= UserConstants.MinPhoneLength)
            {
                errorList.Add($"Длины поля 'Phone' не может быть меньше '{UserConstants.MinPhoneLength}' символов!!");
                return ResultValidation.Failure(errorList);
            }

            if (errorList.Any())
                return ResultValidation.Failure(errorList);

            phoneNumber = new PhoneNumber(normalizedPhone);

            return ResultValidation.Success();
        }

        public bool Equals(PhoneNumber? other)
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

            return Equals((PhoneNumber)obj);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value;

        public static bool operator ==(PhoneNumber? left, PhoneNumber? right) => Equals(left, right);

        public static bool operator !=(PhoneNumber? left, PhoneNumber? right) => !Equals(left, right);
    }
}
