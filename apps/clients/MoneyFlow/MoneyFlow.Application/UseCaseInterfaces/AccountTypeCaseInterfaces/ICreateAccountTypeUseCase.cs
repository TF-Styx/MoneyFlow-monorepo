using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.UseCaseInterfaces.AccountTypeCaseInterfaces
{
    public interface ICreateAccountTypeUseCase
    {
        Task<(AccountTypeDTO AccountTypeDTO, string Message)> CreateAsync(string accountTypeName);
        (AccountTypeDTO AccountTypeDTO, string Message) Create(string accountTypeName);
    }
}