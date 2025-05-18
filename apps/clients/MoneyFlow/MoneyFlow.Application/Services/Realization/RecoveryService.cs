using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;

namespace MoneyFlow.Application.Services.Realization
{
    public class RecoveryService : IRecoveryService
    {
        private readonly IUserService _userService;

        public RecoveryService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<(UserDTO UserDTO, string Message)> RecoveryAsync(string login, string password)
        {
            var user = await _userService.GetAsyncUser(login);
            var idUser = await _userService.UpdateAsyncUser(user.IdUser, user.UserName, user.Avatar, password, user.IdGender);
            var updateUser = await _userService.GetAsyncUser(idUser);

            return (updateUser, "Пароль изменен!!");
        }
        public (UserDTO UserDTO, string Message) Recovery(string login, string password)
        {
            var user = _userService.GetUser(login);
            var idUser = _userService.UpdateUser(user.IdUser, user.UserName, user.Avatar, password, user.IdGender);
            var updateUser = _userService.GetUser(idUser);

            return (updateUser, "Пароль изменен!!");
        }
    }
}
