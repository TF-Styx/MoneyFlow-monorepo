using MoneyFlow.Application.UseCaseInterfaces.FinancialRecordCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.FinancialRecordCases
{
    public class DeleteFinancialRecordUseCase : IDeleteFinancialRecordUseCase
    {
        private readonly IFinancialRecordRepository _financialRecordRepository;

        public DeleteFinancialRecordUseCase(IFinancialRecordRepository financialRecordRepository)
        {
            _financialRecordRepository = financialRecordRepository;
        }

        public async Task<List<int>> DeleteListAsync(int id, bool isDeleteByIdCategory)
        {
            return await _financialRecordRepository.DeleteListAsync(id, isDeleteByIdCategory);
        }

        public async Task<int> DeleteAsync(int idFinancialRecord)
        {
            return await _financialRecordRepository.DeleteAsync(idFinancialRecord);
        }
        public int Delete(int idFinancialRecord)
        {
            return _financialRecordRepository.Delete(idFinancialRecord);
        }
    }
}
