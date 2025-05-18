namespace MoneyFlow.Application.UseCaseInterfaces.BankCaseInterfaces
{
    public interface IDeleteBankUseCase
    {
        Task DeleteAsync(int idBank);
        void Delete(int idBank);
    }
}