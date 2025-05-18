using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.Application.UseCaseInterfaces.FinancialRecordCaseInterfaces;

namespace MoneyFlow.Application.Services.Realization
{
    public class FinancialRecordService : IFinancialRecordService
    {
        private readonly ICreateFinancialRecordUseCase _createFinancialRecordUseCase;
        private readonly IDeleteFinancialRecordUseCase _deleteFinancialRecordUseCase;
        private readonly IGetFinancialRecordUseCase _getFinancialRecordUseCase;
        private readonly IUpdateFinancialRecordUseCase _updateFinancialRecordUseCase;

        public FinancialRecordService(ICreateFinancialRecordUseCase createFinancialRecordUseCase, IDeleteFinancialRecordUseCase deleteFinancialRecordUseCase, IGetFinancialRecordUseCase getFinancialRecordUseCase, IUpdateFinancialRecordUseCase updateFinancialRecordUseCase)
        {
            _createFinancialRecordUseCase = createFinancialRecordUseCase;
            _deleteFinancialRecordUseCase = deleteFinancialRecordUseCase;
            _getFinancialRecordUseCase = getFinancialRecordUseCase;
            _updateFinancialRecordUseCase = updateFinancialRecordUseCase;
        }

        public async Task<(FinancialRecordDTO FinancialRecordDTO, string Message)> CreateAsync(string? recordName, decimal? amount, string? description, int? idTransactionType, int? idUser, int? idCategory, int? idSubcategory, int? idAccount, DateTime? date)
        {
            return await _createFinancialRecordUseCase.CreateAsync(recordName, amount, description, idTransactionType, idUser, idCategory, idSubcategory, idAccount, date);
        }

        public async Task<List<FinancialRecordDTO>> GetAllAsync(int idUser)
        {
            return await _getFinancialRecordUseCase.GetAllAsync(idUser);
        }
        public List<FinancialRecordDTO> GetAll(int idUser)
        {
            return _getFinancialRecordUseCase.GetAll(idUser);
        }

        public async Task<FinancialRecordDTO> GetAsync(int idFinancialRecord)
        {
            return await _getFinancialRecordUseCase.GetAsync(idFinancialRecord);
        }
        public FinancialRecordDTO Get(int idFinancialRecord)
        {
            return _getFinancialRecordUseCase.Get(idFinancialRecord);
        }

        //public async Task<FinancialRecordDTO> GetAsyncFinancialRecord(string recordName)
        //{
        //    return await _getFinancialRecordUseCase.GetAsyncFinancialRecord(recordName);
        //}
        //public FinancialRecordDTO GetFinancialRecord(string recordName)
        //{
        //    return _getFinancialRecordUseCase.GetFinancialRecord(recordName);
        //}

        public async Task<int> UpdateAsync(int idFinancialRecord, string? recordName, decimal? amount, string? description, int? idTransactionType, int? idUser, int? idCategory, int? idSubcategory, int? idAccount, DateTime? date)
        {
            return await _updateFinancialRecordUseCase.UpdateAsync(idFinancialRecord, recordName, amount, description, idTransactionType, idUser, idCategory, idSubcategory, idAccount, date);
        }
        public int Update(int idFinancialRecord, string? recordName, decimal? amount, string? description, int? idTransactionType, int? idUser, int? idCategory, int? idSubcategory, int? idAccount, DateTime? date)
        {
            return _updateFinancialRecordUseCase.Update(idFinancialRecord, recordName, amount, description, idTransactionType, idUser, idCategory, idSubcategory, idAccount, date);
        }

        public async Task<List<int>> DeleteListAsync(int id, bool isDeleteByIdCategory)
        {
            return await _deleteFinancialRecordUseCase.DeleteListAsync(id, isDeleteByIdCategory);
        }

        public async Task<int> DeleteAsync(int idFinancialRecord)
        {
            return await _deleteFinancialRecordUseCase.DeleteAsync(idFinancialRecord);
        }
        public int Delete(int idFinancialRecord)
        {
            return _deleteFinancialRecordUseCase.Delete(idFinancialRecord);
        }
    }
}
