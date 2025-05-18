using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.UseCaseInterfaces.FinancialRecordCaseInterfaces
{
    public interface ICreateFinancialRecordUseCase
    {
        Task<(FinancialRecordDTO FinancialRecordDTO, string Message)> CreateAsync(string? recordName, decimal? amount, string? description, int? idTransactionType, int? idUser, int? idCategory, int? idSubcategory, int? idAccount, DateTime? date);
    }
}