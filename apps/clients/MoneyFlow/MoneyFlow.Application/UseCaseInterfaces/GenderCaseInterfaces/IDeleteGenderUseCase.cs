namespace MoneyFlow.Application.UseCaseInterfaces.GenderCaseInterfaces
{
    public interface IDeleteGenderUseCase
    {
        Task DeleteAsyncGender(int idGender);
        void DeleteGender(int idGender);
    }
}