namespace MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.RoleUseCases
{
    public interface IDeleteRoleUseCase
    {
        Task<int> DeleteAsync(int idRole);
    }
}