using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;

namespace MoneyFlow.Application.Services.Realization
{
    public class AuthorizationService : IAuthorizationService
    {
        public UserDTO CurrentUser { get; private set; }

        private IUserService _userService;

        public AuthorizationService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<(UserDTO UserDTO, string Message)> AuthAsync(string login, string password)
        {
            string message = string.Empty;

            var user = await _userService.GetAsyncUser(login);

            if (user == null)
            {
                return (null, "Данного пользователя нет!!");
            }

            if (user.Password == password)
            {
                CurrentUser = user;
                return (user, message);
            }

            return (null, "Пароль не верный!!");
        }
        public (UserDTO UserDTO, string Message) Auth(string login, string password)
        {
            string message = string.Empty;

            var user = _userService.GetUser(login);

            if (user == null)
            {
                return (null, "Данного пользователя нет!!");
            }

            if (user.Password == password)
            {
                CurrentUser = user;
                return (user, message);
            }

            return (null, "Пароль не верный!!");
        }
    }
}
