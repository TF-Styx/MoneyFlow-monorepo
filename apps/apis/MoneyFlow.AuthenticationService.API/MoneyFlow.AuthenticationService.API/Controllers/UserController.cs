using Microsoft.AspNetCore.Mvc;
using MoneyFlow.AuthenticationService.API.DTOs.Request;
using MoneyFlow.AuthenticationService.API.DTOs.Responses;
using MoneyFlow.AuthenticationService.API.Mapper;
using MoneyFlow.AuthenticationService.Application.DTOs;
using MoneyFlow.AuthenticationService.Application.DTOs.Commands;
using MoneyFlow.AuthenticationService.Application.DTOs.Requests.Response;
using MoneyFlow.AuthenticationService.Application.DTOs.Results;
using MoneyFlow.AuthenticationService.Application.Enums;
using MoneyFlow.AuthenticationService.Application.Providers.Abstraction;
using MoneyFlow.AuthenticationService.Application.Providers.Realization;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.UserUseCases;

namespace MoneyFlow.AuthenticationService.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IRegisterUserUseCase _registerUserUseCase;
        private readonly IAuthenticateUserUseCase _authenticateUserUseCase;
        private readonly IGetUserByLoginUseCase _getUserByLoginUseCase;
        private readonly IDefaultErrorMessageProvider _defaultRegistrationErrorMessageProvider;
        private readonly IDefaultErrorMessageProvider _defaultAuthenticateErrorMessageProvider;

        public UserController(IRegisterUserUseCase createUserUseCase, 
                              IAuthenticateUserUseCase authenticateUserUseCase, 
                              IGetUserByLoginUseCase getUserByLoginUseCase,
                              IEnumerable<IDefaultErrorMessageProvider> messageProvider)
        {
            _registerUserUseCase = createUserUseCase;
            _authenticateUserUseCase = authenticateUserUseCase;
            _getUserByLoginUseCase = getUserByLoginUseCase;
            //_defaultRegistrationErrorMessageProvider = messageProvider.ToList()[0];
            //_defaultAuthenticateErrorMessageProvider = messageProvider.ToList()[1];
            _defaultRegistrationErrorMessageProvider = messageProvider.OfType<DefaultRegistrationErrorMessageProvider>().FirstOrDefault()!;
            _defaultAuthenticateErrorMessageProvider = messageProvider.OfType<DefaultAuthenticateErrorMessageProvider>().FirstOrDefault()!;
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

            UserResult result = await _registerUserUseCase.RegisterAsync(command);

            if (!result.Success)
            {
                Func<IActionResult> errorResponse = result.ErrorCode switch
                {
                    // Ошибка валидации
                    ErrorCode.ValidationFailed => () =>
                    {
                        if (result.ValidationErrors.Any())
                        {
                            var problemDetails = new ValidationProblemDetails
                            {
                                Title = result.ErrorMessage,
                                Status = StatusCodes.Status400BadRequest,
                                Detail = "Указанные вами строки не прошли проверку!!"
                            };

                            problemDetails.Errors.Add(ErrorCode.ValidationFailed.ToString(), result.ValidationErrors.ToArray());

                            return BadRequest(problemDetails);
                            
                        }
                        
                        return BadRequest(new ErrorResponse
                        {
                            ErrorCode = result.ErrorCode.ToString()!,
                            Message = result.ErrorMessage ?? _defaultRegistrationErrorMessageProvider.GetMessage(result.ErrorCode.Value)
                        });
                    },

                    // Ошибка логина || почтового адреса || номера телефона
                    ErrorCode.LoginAlreadyRegistered or 
                    ErrorCode.EmailAlreadyRegistered or 
                    ErrorCode.PhoneAlreadyRegistered => () => Conflict(new ErrorResponse
                    {
                        ErrorCode = result.ErrorCode.ToString()!,
                        Message = result.ErrorMessage ?? _defaultRegistrationErrorMessageProvider.GetMessage(result.ErrorCode.Value)
                    }),

                    // Ошибки которые обычно классифицируются как BadRequest()
                    ErrorCode.WeakPassword or 
                    ErrorCode.InvalidRole or
                    ErrorCode.DomainCreationError => () => BadRequest(new ErrorResponse
                    {
                        ErrorCode = result.ErrorCode.ToString()!,
                        Message = result.ErrorMessage ?? _defaultRegistrationErrorMessageProvider.GetMessage(result.ErrorCode.Value)
                    }),

                    // Ошибки указывающие на проблемы на стороне сервера
                    ErrorCode.SaveUserError or ErrorCode.UnknownError or _ => () =>
                    {
                        // TODO : Добавить логи
                        // Здесь важно залогировать полную информацию

                        return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                        {
                            ErrorCode = result.ErrorCode?.ToString() ?? ErrorCode.UnknownError.ToString(),
                            Message = "Во время регистрации произошла ошибка!!\nПожалуйста попробуйте позже."
                        });
                    }
                };
                return errorResponse.Invoke();
            }

            return StatusCode(StatusCodes.Status201Created, result.User);
        }

        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateUserApiRequest apiRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            UserDTO userDTO;

            AuthenticateUserCommand command = apiRequest.ToMap();

            try
            {
                UserResult result = await _authenticateUserUseCase.AuthenticateAsync(command);

                userDTO = result.User!;

                if (!result.Success)
                {
                    Func<IActionResult> errorResponse = result.ErrorCode switch
                    {
                        // Ошибка валидации
                        ErrorCode.ValidationFailed => () =>
                        {
                            if (result.ValidationErrors.Any())
                            {
                                var problemDetails = new ValidationProblemDetails
                                {
                                    Title = result.ErrorMessage,
                                    Status = StatusCodes.Status400BadRequest,
                                    Detail = "Указанные вами строки не прошли проверку!!"
                                };

                                problemDetails.Errors.Add(ErrorCode.ValidationFailed.ToString(), result.ValidationErrors.ToArray());

                                return BadRequest(problemDetails);

                            }

                            return BadRequest(new ErrorResponse
                            {
                                ErrorCode = result.ErrorCode.ToString()!,
                                Message = result.ErrorMessage ?? _defaultAuthenticateErrorMessageProvider.GetMessage(result.ErrorCode.Value)
                            });
                        },

                        // Ошибка логина
                        ErrorCode.LoginNotExist => () => Conflict(new ErrorResponse
                        {
                            ErrorCode = result.ErrorCode.ToString()!,
                            Message = result.ErrorMessage ?? _defaultAuthenticateErrorMessageProvider.GetMessage(result.ErrorCode.Value)
                        }),

                        // Ошибка пароля
                        ErrorCode.InvalidPassword => () =>
                        {
                            return BadRequest(new ErrorResponse
                            {
                                ErrorCode = result.ErrorCode?.ToString() ?? ErrorCode.UnknownError.ToString(),
                                Message = result.ErrorMessage!
                            });
                        },

                        // Ошибки указывающие на проблемы на стороне сервера
                        ErrorCode.UnknownError or _ => () =>
                        {
                            // TODO : Добавить логи
                            // Здесь важно залогировать полную информацию

                            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                            {
                                ErrorCode = result.ErrorCode?.ToString() ?? ErrorCode.UnknownError.ToString(),
                                Message = "Во время аутентификации произошла непредвиденная ошибка!!\nПожалуйста попробуйте позже. Или обратитесь в поддержку."
                            });
                        }
                    };
                    return errorResponse.Invoke();
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                {
                    ErrorCode = ErrorCode.UnknownError.ToString(),
                    Message = "Во время аутентификации произошла непредвиденная ошибка!!\nПожалуйста попробуйте позже. Или обратитесь в поддержку."
                });
            }

            var userAuthResponse = new UserAuthenticateResponse
            {
                IdUser = userDTO.IdUser,
                Login = userDTO.Login,
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                Phone = userDTO.Phone,
                IdGender = userDTO.IdGender
            };

            return StatusCode(StatusCodes.Status200OK, userAuthResponse);
        }
    }
}