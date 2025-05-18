namespace MoneyFlow.Application.UseCaseInterfaces.AccountTypeCaseInterfaces
{
    public interface IUpdateAccountTypeUseCase
    {
        Task<int> UpdateAsync(int idAccountType, string accountTypeName);
        int Update(int idAccountType, string accountTypeName);
    }
}