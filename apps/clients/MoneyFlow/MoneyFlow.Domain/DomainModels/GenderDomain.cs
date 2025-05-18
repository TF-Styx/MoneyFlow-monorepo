using MoneyFlow.Shared.Constants;

namespace MoneyFlow.Domain.DomainModels
{
    public class GenderDomain
    {
        private GenderDomain(int idGender, string genderName)
        {
            IdGender = idGender;
            GenderName = genderName;
        }

        public int IdGender { get; private set; }
        public string GenderName { get; private set; }

        public static (GenderDomain? GenderDomain, string? Message) Create(int idGender, string genderName)
        {
            var message = string.Empty;

            if (string.IsNullOrWhiteSpace(genderName))
            {
                return (null, "Вы не указали наименование пола!!");
            }

            if (genderName.Length > 0 && char.IsLower(genderName[0]))
            {
                return (null, "Пол указан с маленькой буквы!!");
            }

            if (genderName.Length > IntConstants.MAX_GENDER_NAME_LENGHT)
            {
                return (null, "Превышена длина слова в «20» символов");
            }

            var gender = new GenderDomain(idGender, genderName);

            return (gender, message);
        }
    }
}
