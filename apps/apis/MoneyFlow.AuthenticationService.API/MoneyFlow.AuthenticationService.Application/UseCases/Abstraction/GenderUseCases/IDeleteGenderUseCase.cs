namespace MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.GenderUseCases
{
    public interface IDeleteGenderUseCase
    {
        Task<int> DeleteAsync(int idGender);
    }
}