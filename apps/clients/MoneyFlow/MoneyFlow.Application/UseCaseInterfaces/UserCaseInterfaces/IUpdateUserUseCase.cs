namespace MoneyFlow.Application.UseCaseInterfaces.UserCaseInterfaces
{
    public interface IUpdateUserUseCase
    {
        Task<int> UpdateAsyncUser(int idUser, string? userName, byte[]? avatar, string password, int? idGender);
        int UpdateUser(int idUser, string? userName, byte[]? avatar, string password, int? idGender);
    }
}