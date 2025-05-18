using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Mappers;
using MoneyFlow.Application.UseCaseInterfaces.FinancialRecordCaseInterfaces;
using MoneyFlow.Domain.DomainModels;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.FinancialRecordCases
{
    public class CreateFinancialRecordUseCase : ICreateFinancialRecordUseCase
    {
        private readonly IFinancialRecordRepository _financialRecordRepository;

        public CreateFinancialRecordUseCase(IFinancialRecordRepository financialRecordRepository)
        {
            _financialRecordRepository = financialRecordRepository;
        }

        public async Task<(FinancialRecordDTO FinancialRecordDTO, string Message)> CreateAsync(string? recordName, decimal? amount, string? description, int? idTransactionType, int? idUser, int? idCategory, int? idSubcategory, int? idAccount, DateTime? date)
        {
            var (CreateFinancialRecordDomain, Message) = FinancialRecordDomain.Create
                (
                    0, 
                    recordName, 
                    amount, 
                    description, 
                    idTransactionType, 
                    idUser, 
                    idCategory, 
                    idSubcategory, 
                    idAccount, 
                    date
                );

            var id = await _financialRecordRepository.CreateAsync
                (
                    recordName, 
                    amount, 
                    description, 
                    idTransactionType, 
                    idUser, 
                    idCategory, 
                    idSubcategory, 
                    idAccount, 
                    date
                );

            var domain = await _financialRecordRepository.GetAsync(id);

            return (domain.ToDTO().FinancialRecordDTO, Message);
        }
    }
}
