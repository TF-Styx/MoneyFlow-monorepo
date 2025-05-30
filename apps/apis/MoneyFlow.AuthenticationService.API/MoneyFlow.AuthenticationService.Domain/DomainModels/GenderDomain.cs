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

            var domain = new GenderDomain(idGender, genderName);

            return (domain, message);
        }
    }
}
