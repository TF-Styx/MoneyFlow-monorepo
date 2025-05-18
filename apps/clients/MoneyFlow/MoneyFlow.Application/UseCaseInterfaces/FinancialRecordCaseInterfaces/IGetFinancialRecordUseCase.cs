using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.UseCaseInterfaces.FinancialRecordCaseInterfaces
{
    public interface IGetFinancialRecordUseCase
    {
        Task<List<FinancialRecordDTO>> GetAllAsync(int idUser);
        List<FinancialRecordDTO> GetAll(int idUser);

        Task<FinancialRecordDTO> GetAsync(int idFinancialRecord);
        FinancialRecordDTO Get(int idFinancialRecord);

        //Task<FinancialRecordDTO> GetAsyncFinancialRecord(string recordName);
        //FinancialRecordDTO GetFinancialRecord(string recordName);
    }
}