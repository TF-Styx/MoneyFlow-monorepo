using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.UseCaseInterfaces.UserCaseInterfaces
{
    public interface IGetUserUseCase
    {
        Task<List<UserDTO>> GetAllAsyncUser();
        List<UserDTO> GetAllUser();

        Task<UserDTO> GetAsyncUser(int idUser);
        UserDTO GetUser(int idUser);

        Task<UserDTO> GetAsyncUser(string login); 
        UserDTO GetUser(string login);

        UserTotalInfoDTO GetUserTotalInfo(int idUser);
    }
}