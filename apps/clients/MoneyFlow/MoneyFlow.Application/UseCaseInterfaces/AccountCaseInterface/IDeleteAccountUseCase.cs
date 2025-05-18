namespace MoneyFlow.Application.UseCaseInterfaces.AccountCaseInterface
{
    public interface IDeleteAccountUseCase
    {
        Task DeleteAsync(int idAccount);
        void Delete(int idAccount);
    }
}