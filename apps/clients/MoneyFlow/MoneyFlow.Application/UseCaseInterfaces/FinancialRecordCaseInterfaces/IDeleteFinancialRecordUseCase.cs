namespace MoneyFlow.Application.UseCaseInterfaces.FinancialRecordCaseInterfaces
{
    public interface IDeleteFinancialRecordUseCase
    {
        Task<List<int>> DeleteListAsync(int id, bool isDeleteByIdCategory);
        Task<int> DeleteAsync(int idFinancialRecord);
        int Delete(int idFinancialRecord);
    }
}