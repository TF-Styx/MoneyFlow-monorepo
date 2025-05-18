using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.Services.Abstraction
{
    public interface IUserService
    {
        Task<(UserDTO UserDTO, string Message)> CreateAsyncUser(string userName, string login, string password);
        (UserDTO UserDTO, string Message) CreateUser(string userName, string login, string password);

        Task CreateDefaultRecordAsync(int idUser);
        void CreateDefaultRecord(int idUser);

        Task<List<UserDTO>> GetAllAsyncUser();
        List<UserDTO> GetAllUser();

        Task<UserDTO> GetAsyncUser(int idUser);
        UserDTO GetUser(int idUser);

        Task<UserDTO> GetAsyncUser(string login);
        UserDTO GetUser(string login);

        UserTotalInfoDTO GetUserInfo(int idUser);

        Task<int> UpdateAsyncUser(int idUser, string? userName, byte[]? avatar, string password, int? idGender);
        int UpdateUser(int idUser, string? userName, byte[]? avatar, string password, int? idGender);

        Task DeleteAsyncUser(int idUser);
        void DeleteUser(int idUser);
    }
}