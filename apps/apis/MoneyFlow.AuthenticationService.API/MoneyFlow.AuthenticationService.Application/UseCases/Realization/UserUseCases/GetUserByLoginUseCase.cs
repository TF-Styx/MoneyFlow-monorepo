using MoneyFlow.AuthenticationService.Application.DTOs.Results;
using MoneyFlow.AuthenticationService.Application.Enums;
using MoneyFlow.AuthenticationService.Application.InterfaceRepositories;
using MoneyFlow.AuthenticationService.Application.Mapper;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.UserUseCases;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Realization.UserUseCases
{
    public class GetUserByLoginUseCase : IGetUserByLoginUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetUserByLoginUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResult> GetUserByLogin(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                return UserResult.FailureResult(ErrorCode.ValidationFailed, "Не был указан 'Login'!!");

            var domain = await _userRepository.GetUserByLoginAsync(login);

            if (domain is null)
                return UserResult.FailureResult(ErrorCode.SaveUserError, "Ошибка сохранения пользователя!!");

            var dto = domain.ToDTO();

            return UserResult.SuccessResult(dto);
        }
    }
}
