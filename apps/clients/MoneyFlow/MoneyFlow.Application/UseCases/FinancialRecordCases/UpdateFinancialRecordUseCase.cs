using MoneyFlow.Application.UseCaseInterfaces.FinancialRecordCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.FinancialRecordCases
{
    public class UpdateFinancialRecordUseCase : IUpdateFinancialRecordUseCase
    {
        private readonly IFinancialRecordRepository _financialRecordRepository;

        public UpdateFinancialRecordUseCase(IFinancialRecordRepository financialRecordRepository)
        {
            _financialRecordRepository = financialRecordRepository;
        }

        public async Task<int> UpdateAsync(int idFinancialRecord, string? recordName, decimal? amount, string? description, int? idTransactionType, int? idUser, int? idCategory, int? idSubcategory, int? idAccount, DateTime? date)
        {
            var exist = await _financialRecordRepository.GetAsync(idFinancialRecord);

            if (exist == null)
            {
                throw new Exception("Данной финансовой записи не существует!!");
            }

            return await _financialRecordRepository.UpdateAsync(idFinancialRecord, recordName, amount, description, idTransactionType, idUser, idCategory, idSubcategory, idAccount, date);
        }
        public int Update(int idFinancialRecord, string? recordName, decimal? amount, string? description, int? idTransactionType, int? idUser, int? idCategory, int? idSubcategory, int? idAccount, DateTime? date)
        {
            var exist = _financialRecordRepository.Get(idFinancialRecord);

            if (exist == null)
            {
                throw new Exception("Данной финансовой записи не существует!!");
            }

            return _financialRecordRepository.Update(idFinancialRecord, recordName, amount, description, idTransactionType, idUser, idCategory, idSubcategory, idAccount, date);
        }
    }
}
