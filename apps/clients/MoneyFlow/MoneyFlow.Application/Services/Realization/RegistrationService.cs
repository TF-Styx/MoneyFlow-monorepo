using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;

namespace MoneyFlow.Application.Services.Realization
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUserService _userService;

        public RegistrationService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<(UserDTO UserDTO, string Message)> RegistrationAsync(string userName, string login, string password)
        {
            var (UserDTO, Message) = await _userService.CreateAsyncUser(userName, login, password);
            await _userService.CreateDefaultRecordAsync(UserDTO.IdUser);
            return (UserDTO, Message);
        }
        public (UserDTO UserDTO, string Message) Registration(string userName, string login, string password)
        {
            var (UserDTO, Message) = _userService.CreateUser(userName, login, password);
            _userService.CreateDefaultRecord(UserDTO.IdUser);
            return (UserDTO, Message);
        }
    }
}
