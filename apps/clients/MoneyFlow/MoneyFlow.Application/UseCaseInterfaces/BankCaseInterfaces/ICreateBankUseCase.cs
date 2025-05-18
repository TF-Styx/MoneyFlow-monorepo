using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.UseCaseInterfaces.BankCaseInterfaces
{
    public interface ICreateBankUseCase
    {
        Task<(BankDTO BankDTO, string Message)> CreateAsync(string bankName);
        (BankDTO BankDTO, string Message) Create(string bankName);
    }
}