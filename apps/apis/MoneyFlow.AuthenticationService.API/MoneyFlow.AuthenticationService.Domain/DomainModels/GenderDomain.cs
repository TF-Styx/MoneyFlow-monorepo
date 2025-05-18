using MoneyFlow.AuthenticationService.Domain.Constants;

namespace MoneyFlow.AuthenticationService.Domain.DomainModels
{
    public class GenderDomain
    {
        private GenderDomain(int idGender, string genderName)
        {
            IdGender = idGender;
            GenderName = genderName;
        }

        public int IdGender { get; private set; }
        public string GenderName { get; private set; } = null!;

        public static (GenderDomain GenderDomain, string Message) Create(int idGender, string genderName)
        {
            string message = string.Empty;

            #region Проверки

            if (string.IsNullOrWhiteSpace(genderName))
            {
                message =
                    $"Вы не заполнили поле 'Gender Name'!!\n" +
                    $"Максимальная допустимая длина данного поля '{GenderConstants.MaxGenderNameLength}' символов!!\n" +
                    $"Минимальная допустимая длина данного поля '{GenderConstants.MinGenderNameLength}' символов!!";
                return (null, message);
            }
            if (genderName.Length >= GenderConstants.MaxGenderNameLength)
            {
                message = $"Длина поля 'Gender Name' превышает допустимое значение в '{GenderConstants.MaxGenderNameLength}' символов!!";
                return (null, message);
            }
            if (genderName.Length <= GenderConstants.MinGenderNameLength)
            {
                message = $"Длина поля 'Gender Name' превышает допустимое значение в '{GenderConstants.MinGenderNameLength}' символов!!";
                return (null, message);
            }

            #endregion

            var domain = new GenderDomain(idGender, genderName);

            return (domain, message);
        }
    }
}
