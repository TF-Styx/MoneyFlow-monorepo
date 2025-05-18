using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Mappers;
using MoneyFlow.Application.UseCaseInterfaces.AccountTypeCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.AccountTypeCases
{
    public class GetAccountTypeUseCase : IGetAccountTypeUseCase
    {
        public readonly IAccountTypeRepository _accountTypeRepository;

        public GetAccountTypeUseCase(IAccountTypeRepository accountTypeRepository)
        {
            _accountTypeRepository = accountTypeRepository;
        }

        public async Task<List<AccountTypeDTO>> GetAllAsync()
        {
            var accountsType = await _accountTypeRepository.GetAllAsync();
            var accountsTypeDTO = accountsType.ToListDTO();

            return accountsTypeDTO;
        }
        public List<AccountTypeDTO> GetAll()
        {
            var accountsType = _accountTypeRepository.GetAll();
            var accountsTypeDTO = accountsType.ToListDTO();

            return accountsTypeDTO;
        }

        public async Task<AccountTypeDTO> GetAsync(int idAccountType)
        {
            var accountType = await _accountTypeRepository.GetAsync(idAccountType);

            if (accountType == null) { return null; } // TODO : Сделать исключение

            var accountsTypeDTO = accountType.ToDTO();

            return accountsTypeDTO.AccountTypeDTO;
        }
        public AccountTypeDTO Get(int idAccountType)
        {
            var accountType = _accountTypeRepository.Get(idAccountType);

            if (accountType == null) { return null; } // TODO : Сделать исключение

            var accountsTypeDTO = accountType.ToDTO();

            return accountsTypeDTO.AccountTypeDTO;
        }

        public async Task<AccountTypeDTO> GetAsync(string accountTypeName)
        {
            var accountType = await _accountTypeRepository.GetAsync(accountTypeName);

            if (accountType == null) { return null; } // TODO : Сделать исключение

            var accountsTypeDTO = accountType.ToDTO();

            return accountsTypeDTO.AccountTypeDTO;
        }
        public AccountTypeDTO Get(string accountTypeName)
        {
            var accountType = _accountTypeRepository.Get(accountTypeName);

            if (accountType == null) { return null; } // TODO : Сделать исключение

            var accountsTypeDTO = accountType.ToDTO();

            return accountsTypeDTO.AccountTypeDTO;
        }

        public async Task<UserAccountTypesDTO> GetByIdUserAsync(int idUser)
        {
            var userAccountTypes = await _accountTypeRepository.GetByIdUserAsync(idUser);

            if (userAccountTypes == null) { return null; }

            var userAccountTypesDTO = userAccountTypes.ToDTO();

            return userAccountTypesDTO;
        }
        public UserAccountTypesDTO GetByIdUser(int idUser)
        {
            var userAccountTypes = _accountTypeRepository.GetByIdUser(idUser);

            if (userAccountTypes == null) { return null; }

            var userAccountTypesDTO = userAccountTypes.ToDTO();

            return userAccountTypesDTO;
        }
    }
}
