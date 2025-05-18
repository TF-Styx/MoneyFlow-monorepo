using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.UseCaseInterfaces.UserCaseInterfaces
{
    public interface ICreateUserUseCase
    {
        Task<(UserDTO UserDTO, string Message)> CreateAsyncUser(string userName, string login, string password);
        (UserDTO UserDTO, string Message) CreateUser(string userName, string login, string password);

        Task CreateDefaultRecordAsync(int idUser);
        void CreateDefaultRecord(int idUser);
    }
}