using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Mappers;
using MoneyFlow.Application.UseCaseInterfaces.UserCaseInterfaces;
using MoneyFlow.Domain.DomainModels;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.UserCases
{
    public class CreateUserUseCase : ICreateUserUseCase
    {
        private readonly IUsersRepository _usersRepository;

        public CreateUserUseCase(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<(UserDTO UserDTO, string Message)> CreateAsyncUser(string userName, string login, string password)
        {
            var (CreateUserDomain, Message) = UserDomain.Create(0, userName, null, login, password, 0);

            var existUser = await _usersRepository.GetAsync(CreateUserDomain.Login);

            if (existUser != null) { return (null, "Пользователь с таким логином уже есть!!"); }

            var idUser = await _usersRepository.CreateAsync(userName, login, password);
            var userDomain = await _usersRepository.GetAsync(idUser);

            return (userDomain.ToDTO().UserDTO, Message);
        }
        public (UserDTO UserDTO, string Message) CreateUser(string userName, string login, string password)
        {
            var (CreateUserDomain, Message) = UserDomain.Create(0, userName, null, login, password, 0);

            var existUser = _usersRepository.Get(CreateUserDomain.Login);

            if (existUser != null) { return (null, "Пользователь с таким логином уже есть!!"); }

            var idUser = _usersRepository.Create(userName, login, password);
            var userDomain = _usersRepository.Get(idUser);

            return (userDomain.ToDTO().UserDTO, Message);
        }

        public async Task CreateDefaultRecordAsync(int idUser)
        {
            await _usersRepository.CreateDefaultRecordAsync(idUser);
        }

        public void CreateDefaultRecord(int idUser)
        {
            Task.Run(() => CreateDefaultRecordAsync(idUser));
        }
    }
}
