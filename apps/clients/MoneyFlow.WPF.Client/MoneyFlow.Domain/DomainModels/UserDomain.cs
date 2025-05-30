namespace MoneyFlow.Domain.DomainModels
{
    public class UserDomain
    {
        private UserDomain() { }

        public int IdUser { get; private set; }
        public string Login { get; private set; } = null!;
        public string UserName { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string? Phone { get; private set; }
        public int? IdGender { get; private set; }

        public static UserDomain? Create(string login, string userName, string email, string? phone, int? idGender)
        {
            var domain = new UserDomain()
            {
                Login = login,
                UserName = userName,
                Email = email,
                Phone = phone,
                IdGender = idGender,
            };
            return domain;
        }

        public static UserDomain Reconstitute(int idUser, string login, string userName,
                                              string email, string? phone,
                                              int? idGender)
        {
            return new UserDomain()
            {
                IdUser = idUser,
                Login = login,
                UserName = userName,
                Email = email,
                Phone = phone,
                IdGender = idGender,
            };
        }
    }
}
