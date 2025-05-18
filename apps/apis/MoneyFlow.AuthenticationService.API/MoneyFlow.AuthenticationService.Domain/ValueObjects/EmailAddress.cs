using MoneyFlow.AuthenticationService.Domain.DomainModels.Validations;
using System.Text.RegularExpressions;

namespace MoneyFlow.AuthenticationService.Domain.ValueObjects
{
    public class EmailAddress : IEquatable<EmailAddress>
    {
        private EmailAddress(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static ResultValidation TryCreate(string? email, out EmailAddress? emailAddress)
        {
            emailAddress = null;

            var errorList = new List<string>();

            if (string.IsNullOrWhiteSpace(email))
            {
                errorList.Add("Не заполнено поле 'Email'!!");
                return ResultValidation.Failure(errorList);
            }

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+.[^@\s]+$", RegexOptions.IgnoreCase))
                errorList.Add("Неверный формат 'Email'!!");

            if (errorList.Any())
                return ResultValidation.Failure(errorList);

            emailAddress = new EmailAddress(email.ToLowerInvariant());

            return ResultValidation.Success();
        }

        public bool Equals(EmailAddress? other)
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

            return Equals((EmailAddress)obj);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value;

        public static bool operator ==(EmailAddress? left, EmailAddress? right) => Equals(left, right);

        public static bool operator !=(EmailAddress? left, EmailAddress? right) => !Equals(left, right);
    }
}
