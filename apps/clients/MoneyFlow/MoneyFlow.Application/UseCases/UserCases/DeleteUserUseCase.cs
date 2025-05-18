using MoneyFlow.Application.UseCaseInterfaces.UserCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.UserCases
{
    public class DeleteUserUseCase : IDeleteUserUseCase
    {
        private readonly IUsersRepository _usersRepository;

        public DeleteUserUseCase(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task DeleteAsyncUser(int idUser)
        {
            await _usersRepository.DeleteAsync(idUser); // TODO : Сделать проверку на существование элемента
        }
        public void DeleteUser(int idUser)
        {
            _usersRepository.Delete(idUser); // TODO : Сделать проверку на существование элемента
        }
    }
}
