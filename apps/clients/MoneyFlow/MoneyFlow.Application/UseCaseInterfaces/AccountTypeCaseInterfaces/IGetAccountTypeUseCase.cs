using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.UseCaseInterfaces.AccountTypeCaseInterfaces
{
    public interface IGetAccountTypeUseCase
    {
        Task<List<AccountTypeDTO>> GetAllAsync();
        List<AccountTypeDTO> GetAll();

        Task<AccountTypeDTO> GetAsync(int idAccountType);
        AccountTypeDTO Get(int idAccountType);

        Task<AccountTypeDTO> GetAsync(string accountTypeName);
        AccountTypeDTO Get(string accountTypeName);

        Task<UserAccountTypesDTO> GetByIdUserAsync(int idUser);
        UserAccountTypesDTO GetByIdUser(int idUser);
    }
}