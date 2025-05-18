using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.Application.UseCaseInterfaces.BankCaseInterfaces;

namespace MoneyFlow.Application.Services.Realization
{
    public class BankService : IBankService
    {
        private readonly ICreateBankUseCase _createBankUseCase;
        private readonly IDeleteBankUseCase _deleteBankUseCase;
        private readonly IGetBankUseCase    _getBankUseCase;
        private readonly IUpdateBankUseCase _updateBankUseCase;

        public BankService(ICreateBankUseCase createBankUseCase, IDeleteBankUseCase deleteBankUseCase, IGetBankUseCase getBankUseCase, IUpdateBankUseCase updateBankUseCase)
        {
            _createBankUseCase = createBankUseCase;
            _deleteBankUseCase = deleteBankUseCase;
            _getBankUseCase    = getBankUseCase;
            _updateBankUseCase = updateBankUseCase;
        }

        public async Task<(BankDTO BankDTO, string Message)> CreateAsync(string bankName)
        {
            return await _createBankUseCase.CreateAsync(bankName);
        }
        public (BankDTO BankDTO, string Message) Create(string bankName)
        {
            return _createBankUseCase.Create(bankName);
        }

        public async Task<List<BankDTO>> GetAllAsync()
        {
            return await _getBankUseCase.GetAllAsync();
        }
        public List<BankDTO> GetAll()
        {
            return _getBankUseCase.GetAll();
        }

        public async Task<BankDTO> GetAsync(int idBank)
        {
            return await _getBankUseCase.GetAsync(idBank);
        }
        public BankDTO Get(int idBank)
        {
            return _getBankUseCase.Get(idBank);
        }

        public async Task<BankDTO> GetAsync(string bankName)
        {
            return await _getBankUseCase.GetAsync(bankName);
        }
        public BankDTO Get(string bankName)
        {
            return _getBankUseCase.Get(bankName);
        }

        public async Task<UserBanksDTO> GetByIdUserAsync(int idUser)
        {
            return await _getBankUseCase.GetByIdUserAsync(idUser);
        }
        public UserBanksDTO GetByIdUser(int idUser)
        {
            return _getBankUseCase.GetByIdUser(idUser);
        }

        public async Task<int> UpdateAsync(int idBank, string bankName)
        {
            return await _updateBankUseCase.UpdateAsync(idBank, bankName);
        }
        public int Update(int idBank, string bankName)
        {
            return _updateBankUseCase.Update(idBank, bankName);
        }

        public async Task DeleteAsync(int idBank)
        {
            await _deleteBankUseCase.DeleteAsync(idBank);
        }
        public void Delete(int idBank)
        {
            _deleteBankUseCase.Delete(idBank);
        }
    }
}
