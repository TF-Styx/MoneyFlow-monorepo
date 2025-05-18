using MoneyFlow.Application.UseCaseInterfaces.BankCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.BankCases
{
    public class UpdateBankUseCase : IUpdateBankUseCase
    {
        private readonly IBanksRepository _banksRepository;

        public UpdateBankUseCase(IBanksRepository banksRepository)
        {
            _banksRepository = banksRepository;
        }

        public async Task<int> UpdateAsync(int idBank, string bankName)
        {
            if (string.IsNullOrWhiteSpace(bankName))
            {
                throw new Exception("Данного банка не существует!!");
            }

            var existBank = await _banksRepository.GetAsync(idBank);

            if (existBank == null)
            {
                throw new Exception("Данного банка не существует!!");
            }

            return await _banksRepository.UpdateAsync(idBank, bankName);
        }
        public int Update(int idBank, string bankName)
        {
            if (string.IsNullOrWhiteSpace(bankName))
            {
                throw new Exception("Данного банка не существует!!");
            }

            var existBank = _banksRepository.Get(idBank);

            if (existBank == null)
            {
                throw new Exception("Данного банка не существует!!");
            }

            return _banksRepository.Update(idBank, bankName);
        }
    }
}
