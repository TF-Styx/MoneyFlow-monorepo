using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.Application.UseCaseInterfaces.UserCaseInterfaces;

namespace MoneyFlow.Application.Services.Realization
{
    public class UserService : IUserService
    {
        private readonly ICreateUserUseCase _createUserUseCase;
        private readonly IDeleteUserUseCase _deleteUserCase;
        private readonly IGetUserUseCase    _getUserUseCase;
        private readonly IUpdateUserUseCase _updateUserUseCase;

        public UserService(ICreateUserUseCase createUserUseCase, IDeleteUserUseCase deleteUserCase, IGetUserUseCase getUserUseCase, IUpdateUserUseCase updateUserUseCase)
        {
            _createUserUseCase = createUserUseCase;
            _deleteUserCase    = deleteUserCase;
            _getUserUseCase    = getUserUseCase;
            _updateUserUseCase = updateUserUseCase;
        }

        public async Task<(UserDTO UserDTO, string Message)> CreateAsyncUser(string userName, string login, string password)
        {
            return await _createUserUseCase.CreateAsyncUser(userName, login, password);
        }
        public (UserDTO UserDTO, string Message) CreateUser(string userName, string login, string password)
        {
            return _createUserUseCase.CreateUser(userName, login, password);
        }

        public async Task CreateDefaultRecordAsync(int idUser)
        {
            await _createUserUseCase.CreateDefaultRecordAsync(idUser);
        }

        public void CreateDefaultRecord(int idUser)
        {
            Task.Run(() => CreateDefaultRecordAsync(idUser));
        }

        public async Task<List<UserDTO>> GetAllAsyncUser()
        {
            return await _getUserUseCase.GetAllAsyncUser();
        }
        public List<UserDTO> GetAllUser()
        {
            return _getUserUseCase.GetAllUser();
        }

        public async Task<UserDTO> GetAsyncUser(int idUser)
        {
            return await _getUserUseCase.GetAsyncUser(idUser);
        }
        public UserDTO GetUser(int idUser)
        {
            return _getUserUseCase.GetUser(idUser);
        }

        public async Task<UserDTO> GetAsyncUser(string login)
        {
            return await _getUserUseCase.GetAsyncUser(login);
        }
        public UserDTO GetUser(string login)
        {
            return _getUserUseCase.GetUser(login);
        }

        public UserTotalInfoDTO GetUserInfo(int idUser)
        {
            return _getUserUseCase.GetUserTotalInfo(idUser);
        }

        public async Task<int> UpdateAsyncUser(int idUser, string? userName, byte[]? avatar,
                                      string password, int? idGender)
        {
            return await _updateUserUseCase.UpdateAsyncUser(idUser, userName, avatar, password, idGender);
        }
        public int UpdateUser(int idUser, string? userName, byte[]? avatar,
                                      string password, int? idGender)
        {
            return _updateUserUseCase.UpdateUser(idUser, userName, avatar, password, idGender);
        }

        public async Task DeleteAsyncUser(int idUser)
        {
            await _deleteUserCase.DeleteAsyncUser(idUser);
        }
        public void DeleteUser(int idUser)
        {
            _deleteUserCase.DeleteAsyncUser(idUser);
        }
    }
}       
        