namespace MoneyFlow.Application.UseCaseInterfaces.BankCaseInterfaces
{
    public interface IUpdateBankUseCase
    {
        Task<int> UpdateAsync(int idBank, string bankName);
        int Update(int idBank, string bankName);
    }
}