using Microsoft.AspNetCore.Mvc;
using MoneyFlow.AuthenticationService.API.DTOs.Request;
using MoneyFlow.AuthenticationService.API.Mapper;
using MoneyFlow.AuthenticationService.Application.DTOs.Commands;
using MoneyFlow.AuthenticationService.Application.DTOs.Requests.Response;
using MoneyFlow.AuthenticationService.Application.DTOs.Results;
using MoneyFlow.AuthenticationService.Application.Enums;
using MoneyFlow.AuthenticationService.Application.Providers.Abstraction;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.UserUseCases;

namespace MoneyFlow.AuthenticationService.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class RegisterUserController : ControllerBase
    {
        private readonly ICreateUserUseCase _createUserUseCase;
        private readonly IDefaultRegistrationErrorMessageProvider _defaultRegistrationErrorMessageProvider;

        public RegisterUserController(ICreateUserUseCase createUserUseCase, IDefaultRegistrationErrorMessageProvider defaultRegistrationErrorMessageProvider)
        {
            _createUserUseCase = createUserUseCase;
            _defaultRegistrationErrorMessageProvider = defaultRegistrationErrorMessageProvider;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] RegisterUserApiRequest apiRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            RegisterUserCommand command = apiRequest.ToMap();

            RegisterUserResult result = await _createUserUseCase.CreateAsync(command);

            if (!result.Success)
            {
                Func<IActionResult> errorResponse = result.ErrorCode switch
                {
                    // Ошибка валидации
                    RegistrationErrorCode.ValidationFailed => () =>
                    {
                        if (result.ValidationErrors.Any())
                        {
                            var problemDetails = new ValidationProblemDetails
                            {
                                Title = result.ErrorMessage,
                                Status = StatusCodes.Status400BadRequest,
                                Detail = "Указанные вами строки не прошли проверку!!"
                            };

                            problemDetails.Errors.Add(RegistrationErrorCode.ValidationFailed.ToString(), result.ValidationErrors.ToArray());

                            return BadRequest(problemDetails);
                            
                        }
                        
                        return BadRequest(new ErrorResponse
                        {
                            ErrorCode = result.ErrorCode.ToString()!,
                            Message = result.ErrorMessage ?? _defaultRegistrationErrorMessageProvider.GetMessage(result.ErrorCode.Value)
                        });
                    },

                    // Ошибка логина
                    RegistrationErrorCode.LoginAlreadyTaken or 
                    RegistrationErrorCode.EmailAlreadyRegistered or 
                    RegistrationErrorCode.PhoneAlreadyRegistered => () => Conflict(new ErrorResponse
                    {
                        ErrorCode = result.ErrorCode.ToString()!,
                        Message = result.ErrorMessage ?? _defaultRegistrationErrorMessageProvider.GetMessage(result.ErrorCode.Value)
                    }),

                    // Ошибки которые обычно классифицируются как BadRequest()
                    RegistrationErrorCode.WeakPassword or 
                    RegistrationErrorCode.InvalidRole or
                    RegistrationErrorCode.DomainCreationError => () => BadRequest(new ErrorResponse
                    {
                        ErrorCode = result.ErrorCode.ToString()!,
                        Message = result.ErrorMessage ?? _defaultRegistrationErrorMessageProvider.GetMessage(result.ErrorCode.Value)
                    }),

                    // Ошибки указывающие на проблемы на стороне сервера
                    RegistrationErrorCode.SaveUserError or RegistrationErrorCode.UnknownError or _ => () =>
                    {
                        // TODO : Добавить логи
                        // Здесь важно залогировать полную информацию

                        return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                        {
                            ErrorCode = result.ErrorCode?.ToString() ?? RegistrationErrorCode.UnknownError.ToString(),
                            Message = "Во время регистрации произошла ошибка!!\nПожалуйста попробуйте позже."
                        });
                    }
                };
                return errorResponse.Invoke();
            }

            return StatusCode(StatusCodes.Status201Created, result.User);
        }
    }
}