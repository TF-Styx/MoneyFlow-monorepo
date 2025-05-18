namespace MoneyFlow.Application.UseCaseInterfaces.AccountTypeCaseInterfaces
{
    public interface IDeleteAccountTypeUseCase
    {
        Task DeleteAsync(int idAccountType);
        void Delete(int idAccountType);
    }
}