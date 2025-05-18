using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Mappers;
using MoneyFlow.Application.UseCaseInterfaces.UserCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.UserCases
{
    public class GetUserUseCase : IGetUserUseCase
    {
        private readonly IUsersRepository _usersRepository;

        public GetUserUseCase(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<List<UserDTO>> GetAllAsyncUser()
        {
            var users = await _usersRepository.GetAllAsync();
            var usersDTO = users.ToListDTO();

            return usersDTO;
        }
        public List<UserDTO> GetAllUser()
        {
            var users = _usersRepository.GetAll();
            var usersDTO = users.ToListDTO();

            return usersDTO;
        }

        public async Task<UserDTO> GetAsyncUser(int idUser)
        {
            var user = await _usersRepository.GetAsync(idUser);

            if (user == null) { return null; } // TODO : Сделать исключение

            var userDTO = user.ToDTO();

            return userDTO.UserDTO;
        }
        public UserDTO GetUser(int idUser)
        {
            var user = _usersRepository.Get(idUser);

            if (user == null) { return null; } // TODO : Сделать исключение

            var userDTO = user.ToDTO();

            return userDTO.UserDTO;
        }

        public async Task<UserDTO> GetAsyncUser(string login)
        {
            var user = await _usersRepository.GetAsync(login);

            if (user == null) { return null; } // TODO : Сделать исключение

            var userDTO = user.ToDTO();

            return userDTO.UserDTO;
        }
        public UserDTO GetUser(string login)
        {
            var user = _usersRepository.Get(login);

            if (user == null) { return null; } // TODO : Сделать исключение

            var userDTO = user.ToDTO();

            return userDTO.UserDTO;
        }

        public UserTotalInfoDTO GetUserTotalInfo(int idUser)
        {
            var user = _usersRepository.GetTotalUserInfo(idUser);

            if (user == null) { return null; }

            var userInfoDTO = user.ToDTO();

            return userInfoDTO.UserTotalInfoDTO;
        }
    }
}
