namespace MoneyFlow.Application.UseCaseInterfaces.UserCaseInterfaces
{
    public interface IDeleteUserUseCase
    {
        Task DeleteAsyncUser(int idUser);
        void DeleteUser(int idUser);
    }
}