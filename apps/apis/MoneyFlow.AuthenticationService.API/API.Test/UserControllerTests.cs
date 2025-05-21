using Microsoft.AspNetCore.Mvc;
using MoneyFlow.AuthenticationService.API.Controllers;
using MoneyFlow.AuthenticationService.API.DTOs.Request;
using MoneyFlow.AuthenticationService.API.DTOs.Responses;
using MoneyFlow.AuthenticationService.Application.DTOs;
using MoneyFlow.AuthenticationService.Application.DTOs.Commands;
using MoneyFlow.AuthenticationService.Application.DTOs.Requests.Response;
using MoneyFlow.AuthenticationService.Application.DTOs.Results;
using MoneyFlow.AuthenticationService.Application.Enums;
using MoneyFlow.AuthenticationService.Application.Providers.Abstraction;
using MoneyFlow.AuthenticationService.Application.Providers.Realization;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.UserUseCases;
using Moq;

namespace API.Test
{
    [TestFixture]
    internal class UserControllerTests
    {
        private Mock<IRegisterUserUseCase> _registerUserUseCaseMock;
        private Mock<IAuthenticateUserUseCase> _authenticateUserUseCaseMock;
        private Mock<IRecoveryAccessUserUseCase> _recoveryAccessUserUseCaseMock;
        private Mock<IGetUserByLoginUseCase> _getUserByLoginUseCaseMock;
        private Mock<IDefaultErrorMessageProvider> _defaultRegistrationErrorMessageProviderMock;
        private Mock<IDefaultErrorMessageProvider> _defaultAuthenticateErrorMessageProviderMock;
        private UserController _controller;

        [SetUp]
        public void SetUp()
        {
            var listDefaultErrorMessageProvider = new List<IDefaultErrorMessageProvider>()
            {
                new DefaultRegistrationErrorMessageProvider(),
                new DefaultAuthenticateErrorMessageProvider()
            };

            _registerUserUseCaseMock = new Mock<IRegisterUserUseCase>();
            _authenticateUserUseCaseMock = new Mock<IAuthenticateUserUseCase>();
            _recoveryAccessUserUseCaseMock = new Mock<IRecoveryAccessUserUseCase>();
            _getUserByLoginUseCaseMock = new Mock<IGetUserByLoginUseCase>();
            _defaultRegistrationErrorMessageProviderMock = new Mock<IDefaultErrorMessageProvider>();
            _defaultAuthenticateErrorMessageProviderMock = new Mock<IDefaultErrorMessageProvider>();
            _controller = new UserController
                (
                    _registerUserUseCaseMock.Object, 
                    _authenticateUserUseCaseMock.Object, 
                    _recoveryAccessUserUseCaseMock.Object,
                    _getUserByLoginUseCaseMock.Object,
                    listDefaultErrorMessageProvider
                );
        }

        private static UserDTO CreateSampleUserDTO(string? login = "Styx", string? userName = "Семён", string password = "ValidPass123.!", string? email = "ValidEmail@mail.ru", string? phone = "+7004001010")
        {
            return new UserDTO()
            {
                IdUser = 1,
                Login = login,
                UserName = userName,
                Email = email,
                Phone = phone,
                IdGender = 1,
            };
        }

        private static RegisterUserApiRequest CreateSampleRegisterUserApiRequest(string? login = "Styx", string? userName = "Семён", string password = "ValidPass123.!", string? email = "ValidEmail@mail.ru", string? phone = "+7004001010")
        {
            return new RegisterUserApiRequest()
            {
                Login = login,
                UserName = userName,
                Password = password,
                Email = email,
                Phone = phone,
                IdGender = 1,
            };
        }

        private static AuthenticateUserApiRequest CreateSampleAuthenticateUserApiRequest(string? login = "Styx", string password = "ValidPass123.!")
        {
            return new AuthenticateUserApiRequest()
            {
                Login = login,
                Password = password,
            };
        }

        private static RecoveryAccessUserApiRequest CreateSampleRecoveryAccessUserApiRequest(string email = "ValidEmail@mail.ru", string? login = "Styx", string password = "ValidPass123.!")
        {
            return new RecoveryAccessUserApiRequest()
            {
                Email = email,
                Login = login,
                NewPassword = password,
            };
        }

        #region Register

        [Test]
        public async Task CreateAsync_ValidRequest_ReturnCreateStatus201WithUser()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest();

            var userDTO = CreateSampleUserDTO();
            var result = UserResult.SuccessResult(userDTO);

            _registerUserUseCaseMock.Setup(x => x.RegisterAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);

            var actionResult = await _controller.Register(apiRequest);
            var createActionResult = (ObjectResult)actionResult;

            Assert.That(actionResult, Is.TypeOf<ObjectResult>(), "Должен вернуть 'Статус 201'");
            Assert.That(createActionResult.StatusCode, Is.EqualTo(201), "Статус код должен быть 201");
            Assert.That(createActionResult.Value, Is.EqualTo(userDTO), "Возвращаемое значение должно быть UserDTO");
        }

        [Test]
        public async Task CreateAsync_InvalidModelState_ReturnBadRequest()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest();
            _controller.ModelState.AddModelError("Login", "Данный логин уже занят!!");

            var actionResult = await _controller.Register(apiRequest);
            var badRequest = (BadRequestObjectResult)actionResult;

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>(), "Должен вернуться BadRequest статус 400");
            Assert.That(badRequest.StatusCode, Is.EqualTo(400), "Статус код должен быть 400");
            Assert.That(badRequest.Value, Is.TypeOf<SerializableError>(), "Должен вернуть ModelsState ошибку");
        }

        [Test]
        public async Task CreateAsync_ValidationFailed_ReturnBadRequest_WithValidationErrors()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest(login: " ");

            var validationErrors = new List<string> { "Вы не заполнили поле 'Login'!!", "Пароль не достаточно надежен!!" };
            var result = UserResult.ValidationFailureResult(validationErrors, "Ошибка валидации!!");

            _registerUserUseCaseMock.Setup(x => x.RegisterAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(ErrorCode.ValidationFailed)).Returns("Ошибка валидации данных!!");

            var actionResult = await _controller.Register(apiRequest);
            var badRequestResult = (BadRequestObjectResult)actionResult;
            var problemDetails = (ValidationProblemDetails)badRequestResult.Value!;

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(problemDetails.Errors[ErrorCode.ValidationFailed.ToString()], Contains.Item("Вы не заполнили поле 'Login'!!"));
            Assert.That(problemDetails.Errors[ErrorCode.ValidationFailed.ToString()], Contains.Item("Пароль не достаточно надежен!!"));
        }

        [Test]
        public async Task CreateAsync_ValidationFailed_ReturnBadRequest_WithErrorMessage()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest();

            var result = UserResult.FailureResult(ErrorCode.ValidationFailed, "Ошибка валидации!!");

            _registerUserUseCaseMock.Setup(x => x.RegisterAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(ErrorCode.ValidationFailed)).Returns("Ошибка валидации данных!!");

            var actionResult = await _controller.Register(apiRequest);
            var badRequestResult = (BadRequestObjectResult)actionResult;
            var errorResponse = (ErrorResponse)badRequestResult.Value!;

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(errorResponse, Is.TypeOf<ErrorResponse>());
            Assert.That(errorResponse.ErrorCode, Is.EqualTo(ErrorCode.ValidationFailed.ToString()));
            Assert.That(errorResponse.Message, Is.EqualTo("Ошибка валидации!!"));
        }

        [Test]
        public async Task CreateAsync_LoginAlreadyTaken_ReturnConflict()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest(login: "TestExistLogin");
            
            var result = UserResult.FailureResult(ErrorCode.LoginAlreadyRegistered, $"Логин: '{apiRequest.Login}' уже занят, придумайте новый!!");

            _registerUserUseCaseMock.Setup(x => x.RegisterAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(ErrorCode.LoginAlreadyRegistered)).Returns("Этот логин уже занят");

            var actionResult = await _controller.Register(apiRequest);
            var conflictRequestResult = (ConflictObjectResult)actionResult;
            var resultValue = (ErrorResponse)conflictRequestResult.Value!;

            TestContext.Out.WriteLine(conflictRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<ConflictObjectResult>());
            Assert.That(conflictRequestResult.StatusCode, Is.EqualTo(409));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(ErrorCode.LoginAlreadyRegistered.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo($"Логин: '{apiRequest.Login}' уже занят, придумайте новый!!"));
        }

        [Test]
        public async Task CreateAsync_EmailAlreadyRegistered_ReturnConflict()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest();
            
            var result = UserResult.FailureResult(ErrorCode.EmailAlreadyRegistered, $"'Email': '{apiRequest.Email}' уже занят!!");

            _registerUserUseCaseMock.Setup(x => x.RegisterAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(ErrorCode.EmailAlreadyRegistered)).Returns("Этот почтовый адрес уже занят");

            var actionResult = await _controller.Register(apiRequest);
            var conflictRequestResult = (ConflictObjectResult)actionResult;
            var resultValue = (ErrorResponse)conflictRequestResult.Value!;

            TestContext.Out.WriteLine(conflictRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<ConflictObjectResult>());
            Assert.That(conflictRequestResult.StatusCode, Is.EqualTo(409));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(ErrorCode.EmailAlreadyRegistered.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo($"'Email': '{apiRequest.Email}' уже занят!!"));
        }

        [Test]
        public async Task CreateAsync_PhoneAlreadyRegistered_ReturnConflict()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest();
            
            var result = UserResult.FailureResult(ErrorCode.PhoneAlreadyRegistered, $"Данный 'Phone': '{apiRequest.Phone}' уже занят!!");

            _registerUserUseCaseMock.Setup(x => x.RegisterAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(ErrorCode.PhoneAlreadyRegistered)).Returns("Этот номер телефона уже зарегистрирован");

            var actionResult = await _controller.Register(apiRequest);
            var conflictRequestResult = (ConflictObjectResult)actionResult;
            var resultValue = (ErrorResponse)conflictRequestResult.Value!;

            TestContext.Out.WriteLine(conflictRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<ConflictObjectResult>());
            Assert.That(conflictRequestResult.StatusCode, Is.EqualTo(409));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(ErrorCode.PhoneAlreadyRegistered.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo($"Данный 'Phone': '{apiRequest.Phone}' уже занят!!"));
        }

        [Test]
        public async Task CreateAsync_WeakPassword_ReturnBadRequest()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest();

            var result = UserResult.FailureResult(ErrorCode.WeakPassword, $"Ваш пароль слишком легкий");

            _registerUserUseCaseMock.Setup(x => x.RegisterAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(ErrorCode.WeakPassword)).Returns("Недостаточная безопасность пароля!!");

            var actionResult = await _controller.Register(apiRequest);
            var badRequestResult = (BadRequestObjectResult)actionResult;
            var resultValue = (ErrorResponse)badRequestResult.Value!;

            TestContext.Out.WriteLine(badRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(ErrorCode.WeakPassword.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo("Ваш пароль слишком легкий"));
        }

        [Test]
        public async Task CreateAsync_InvalidRole_ReturnBadRequest()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest();

            var result = UserResult.FailureResult(ErrorCode.InvalidRole, "Не указана роль");

            _registerUserUseCaseMock.Setup(x => x.RegisterAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(ErrorCode.InvalidRole)).Returns("Не назначена роль!!");

            var actionResult = await _controller.Register(apiRequest);
            var badRequestResult = (BadRequestObjectResult)actionResult;
            var resultValue = (ErrorResponse)badRequestResult.Value!;

            TestContext.Out.WriteLine(badRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(ErrorCode.InvalidRole.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo("Не указана роль"));
        }

        [Test]
        public async Task CreateAsync_DomainCreationError_ReturnBadRequest()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest();

            var result = UserResult.FailureResult(ErrorCode.DomainCreationError, "Не создана DomainModel");

            _registerUserUseCaseMock.Setup(x => x.RegisterAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(ErrorCode.DomainCreationError)).Returns("Не удалось создать DomainModel!!");

            var actionResult = await _controller.Register(apiRequest);
            var badRequestResult = (BadRequestObjectResult)actionResult;
            var resultValue = (ErrorResponse)badRequestResult.Value!;

            TestContext.Out.WriteLine(badRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(ErrorCode.DomainCreationError.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo("Не создана DomainModel"));
        }

        [Test]
        public async Task CreateAsync_SaveUserError_ReturnBadRequest()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest();

            var result = UserResult.FailureResult(ErrorCode.SaveUserError);

            _registerUserUseCaseMock.Setup(x => x.RegisterAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(ErrorCode.SaveUserError)).Returns("Не удалось сохранить пользователя!!");

            var actionResult = await _controller.Register(apiRequest);
            var badRequestResult = (ObjectResult)actionResult;
            var resultValue = (ErrorResponse)badRequestResult.Value!;

            TestContext.Out.WriteLine(badRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<ObjectResult>());
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(500));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(ErrorCode.SaveUserError.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo("Во время регистрации произошла ошибка!!\nПожалуйста попробуйте позже."));
        }

        [Test]
        public async Task CreateAsync_UnknownError_ReturnBadRequest()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest();

            var result = UserResult.FailureResult(ErrorCode.UnknownError);

            _registerUserUseCaseMock.Setup(x => x.RegisterAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(ErrorCode.UnknownError)).Returns("Неизвестная ошибка!!");

            var actionResult = await _controller.Register(apiRequest);
            var badRequestResult = (ObjectResult)actionResult;
            var resultValue = (ErrorResponse)badRequestResult.Value!;

            TestContext.Out.WriteLine(badRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<ObjectResult>());
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(500));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(ErrorCode.UnknownError.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo("Во время регистрации произошла ошибка!!\nПожалуйста попробуйте позже."));
        }

        [Test]
        public async Task CreateAsync_None_ReturnBadRequest()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest();

            var result = UserResult.FailureResult(ErrorCode.None);

            _registerUserUseCaseMock.Setup(x => x.RegisterAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(ErrorCode.None)).Returns("Неизвестная ошибка!!");

            var actionResult = await _controller.Register(apiRequest);
            var badRequestResult = (ObjectResult)actionResult;
            var resultValue = (ErrorResponse)badRequestResult.Value!;

            TestContext.Out.WriteLine(badRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<ObjectResult>());
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(500));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(ErrorCode.None.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo("Во время регистрации произошла ошибка!!\nПожалуйста попробуйте позже."));
        }

        #endregion


        #region Authenticate

        [Test]
        public async Task AuthenticateAsync_ValidRequest_ReturnAuthenticateStatus200OK_WithUserAuthenticateResponse()
        {
            var apiRequest = CreateSampleAuthenticateUserApiRequest();

            var userDTO = CreateSampleUserDTO();
            var result = UserResult.SuccessResult(userDTO);

            _authenticateUserUseCaseMock.Setup(x => x.AuthenticateAsync(It.IsAny<AuthenticateUserCommand>())).ReturnsAsync(result);

            var actionResult = await _controller.Authenticate(apiRequest);
            var authResult = (ObjectResult)actionResult;
            var authResponse = (UserAuthenticateResponse?)authResult.Value;

            TestContext.Out.WriteLine(authResponse.Login);

            Assert.That(actionResult, Is.TypeOf<ObjectResult>(), "Должен вернуть 'ObjectResult'");
            Assert.That(authResult.StatusCode, Is.EqualTo(200), "Статус код должен быть 200");
            Assert.That(authResult.Value, Is.Not.Null, "Возвращаемое значение должно быть пустым");
        }

        [Test]
        public async Task AuthenticateAsync_InvalidModelState_ReturnBadRequest()
        {
            var apiRequest = CreateSampleAuthenticateUserApiRequest();
            _controller.ModelState.AddModelError("Login", "Данный логин не найден!!");

            var actionResult = await _controller.Authenticate(apiRequest);
            var badRequest = (BadRequestObjectResult)actionResult;

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>(), "Должен вернуться BadRequest статус 400");
            Assert.That(badRequest.StatusCode, Is.EqualTo(400), "Статус код должен быть 400");
            Assert.That(badRequest.Value, Is.TypeOf<SerializableError>(), "Должен вернуть ModelsState ошибку");
        }

        [Test]
        public async Task AuthenticateAsync_ValidationFailed_ReturnBadRequest_WithValidationErrors()
        {
            var apiRequest = CreateSampleAuthenticateUserApiRequest(login: " ", password: " ");

            var validationErrors = new List<string> { "Вы не заполнили поле 'Login'!!", "Вы не заполнили поле 'Password'!!" };
            var result = UserResult.ValidationFailureResult(validationErrors, "Ошибка валидации!!");

            _authenticateUserUseCaseMock.Setup(x => x.AuthenticateAsync(It.IsAny<AuthenticateUserCommand>())).ReturnsAsync(result);
            _defaultAuthenticateErrorMessageProviderMock.Setup(x => x.GetMessage(ErrorCode.ValidationFailed)).Returns("Ошибка валидации данных!!");

            var actionResult = await _controller.Authenticate(apiRequest);
            var badRequestResult = (BadRequestObjectResult)actionResult;
            var problemDetails = (ValidationProblemDetails)badRequestResult.Value!;

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(problemDetails.Errors[ErrorCode.ValidationFailed.ToString()], Contains.Item("Вы не заполнили поле 'Login'!!"));
            Assert.That(problemDetails.Errors[ErrorCode.ValidationFailed.ToString()], Contains.Item("Вы не заполнили поле 'Password'!!"));
        }

        [Test]
        public async Task AuthenticateAsync_LoginNotExist_ReturnConflict()
        {
            var apiRequest = CreateSampleAuthenticateUserApiRequest(login: "TestExistLogin");

            var result = UserResult.FailureResult(ErrorCode.LoginNotExist, $"Логин: '{apiRequest.Login}' не существует!!");

            _authenticateUserUseCaseMock.Setup(x => x.AuthenticateAsync(It.IsAny<AuthenticateUserCommand>())).ReturnsAsync(result);
            _defaultAuthenticateErrorMessageProviderMock.Setup(x => x.GetMessage(ErrorCode.LoginNotExist)).Returns("Данного логина не существует!!");

            var actionResult = await _controller.Authenticate(apiRequest);
            var conflictRequestResult = (ConflictObjectResult)actionResult;
            var resultValue = (ErrorResponse)conflictRequestResult.Value!;

            TestContext.Out.WriteLine(conflictRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<ConflictObjectResult>());
            Assert.That(conflictRequestResult.StatusCode, Is.EqualTo(409));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(ErrorCode.LoginNotExist.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo($"Логин: '{apiRequest.Login}' не существует!!"));
        }

        [Test]
        public async Task AuthenticateAsync_InvalidPassword_ReturnBadRequest()
        {
            var apiRequest = CreateSampleAuthenticateUserApiRequest();

            var result = UserResult.FailureResult(ErrorCode.InvalidPassword, $"Не верный пароль!!");

            _authenticateUserUseCaseMock.Setup(x => x.AuthenticateAsync(It.IsAny<AuthenticateUserCommand>())).ReturnsAsync(result);
            _defaultAuthenticateErrorMessageProviderMock.Setup(x => x.GetMessage(ErrorCode.InvalidPassword)).Returns("Не верный пароль!!");

            var actionResult = await _controller.Authenticate(apiRequest);
            var conflictRequestResult = (BadRequestObjectResult)actionResult;
            var resultValue = (ErrorResponse)conflictRequestResult.Value!;

            TestContext.Out.WriteLine(conflictRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(conflictRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(ErrorCode.InvalidPassword.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo($"Не верный пароль!!"));
        }

        [Test]
        public async Task AuthenticateAsync_UnknownError_ReturnBadRequest()
        {
            var apiRequest = CreateSampleAuthenticateUserApiRequest();

            var result = UserResult.FailureResult(ErrorCode.UnknownError, "Во время аутентификации произошла непредвиденная ошибка!!\nПожалуйста попробуйте позже. Или обратитесь в поддержку.");

            _authenticateUserUseCaseMock.Setup(x => x.AuthenticateAsync(It.IsAny<AuthenticateUserCommand>())).ReturnsAsync(result);
            _defaultAuthenticateErrorMessageProviderMock.Setup(x => x.GetMessage(ErrorCode.UnknownError)).Returns("Неизвестная ошибка!!");

            var actionResult = await _controller.Authenticate(apiRequest);
            var badRequestResult = (ObjectResult)actionResult;
            var resultValue = (ErrorResponse)badRequestResult.Value!;

            TestContext.Out.WriteLine(badRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<ObjectResult>());
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(500));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(ErrorCode.UnknownError.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo("Во время аутентификации произошла непредвиденная ошибка!!\nПожалуйста попробуйте позже. Или обратитесь в поддержку."));
        }

        [Test]
        public async Task AuthenticateAsync_UnknownError_ThrowsException()
        {
            var apiRequest = CreateSampleAuthenticateUserApiRequest();

            var result = UserResult.FailureResult(ErrorCode.UnknownError, "Во время аутентификации произошла непредвиденная ошибка!!\nПожалуйста попробуйте позже. Или обратитесь в поддержку.");

            _authenticateUserUseCaseMock.Setup(x => x.AuthenticateAsync(It.IsAny<AuthenticateUserCommand>())).ThrowsAsync( new ArgumentNullException());
            _defaultAuthenticateErrorMessageProviderMock.Setup(x => x.GetMessage(ErrorCode.UnknownError)).Returns("Неизвестная ошибка!!");

            var actionResult = await _controller.Authenticate(apiRequest);
            var badRequestResult = (ObjectResult)actionResult;
            var resultValue = (ErrorResponse)badRequestResult.Value!;

            TestContext.Out.WriteLine(badRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<ObjectResult>());
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(500));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(ErrorCode.UnknownError.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo("Во время аутентификации произошла непредвиденная ошибка!!\nПожалуйста попробуйте позже. Или обратитесь в поддержку."));
        }

        #endregion


        #region RecoveryAccess

        [Test]
        public async Task RecoveryAccessAsync_ValidRequest_ReturnAuthenticateStatus200OK_WithUserAuthenticateResponse()
        {
            var apiRequest = CreateSampleRecoveryAccessUserApiRequest();
            var result = UserResult.SuccessResult();

            _recoveryAccessUserUseCaseMock.Setup(x => x.RecoveryAccessAsync(It.IsAny<RecoveryAccessUserCommand>())).ReturnsAsync(result);

            var actionResult = await _controller.RecoveryAccess(apiRequest);
            var authResult = (ObjectResult)actionResult;
            var authResponse = (bool?)authResult.Value;

            Assert.Multiple(() =>
            {
                Assert.That(actionResult, Is.TypeOf<ObjectResult>(), "Должен вернуть 'ObjectResult'");
                Assert.That(authResult.StatusCode, Is.EqualTo(200), "Статус код должен быть 200");
                Assert.That(authResult.Value, Is.Not.Null, "Возвращаемое значение должно быть пустым");
                Assert.That(authResult.Value, Is.TypeOf<bool>(), "Тип bool");
            });
        }

        [Test]
        public async Task RecoveryAccessAsync_InvalidModelState_ReturnBadRequest()
        {
            _controller.ModelState.AddModelError("Ошибка!!", "Непредвиденная ошибка!!");

            var apiRequest = CreateSampleRecoveryAccessUserApiRequest();

            var actionResult = await _controller.RecoveryAccess(apiRequest);
            var result = (BadRequestObjectResult)actionResult;

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
                Assert.That(result.StatusCode, Is.EqualTo(400));
                Assert.That(result.Value, Is.TypeOf<SerializableError>());
            });
        }

        [Test]
        public async Task RecoveryAccessAsync_ValidationFailed_ReturnBadRequestWithValidationErrors()
        {
            var apiRequest = CreateSampleRecoveryAccessUserApiRequest(email: " ", login: " ", password: " ");

            var validationErrors = new List<string>() { "Вы не заполнили поле 'Email'", "Вы не заполнили поле 'Login'", "Вы не заполнили поле 'NewPassword'" };
            var result = UserResult.ValidationFailureResult(validationErrors, "Ошибки валидации!!");

            _recoveryAccessUserUseCaseMock.Setup(x => x.RecoveryAccessAsync(It.IsAny<RecoveryAccessUserCommand>())).ReturnsAsync(result);
            _defaultAuthenticateErrorMessageProviderMock.Setup(x => x.GetMessage(ErrorCode.ValidationFailed)).Returns("Ошибка валидации данных!!");

            var actionResult = await _controller.RecoveryAccess(apiRequest);
            var badRequestResult = (BadRequestObjectResult)actionResult;
            var details = (ValidationProblemDetails)badRequestResult.Value!;

            Assert.Multiple(() =>
            {
                Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
                Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
                Assert.That(details.Title, Is.EqualTo("Ошибки валидации!!"));
                Assert.That(details.Errors[ErrorCode.ValidationFailed.ToString()], Contains.Item("Вы не заполнили поле 'Email'"));
                Assert.That(details.Errors[ErrorCode.ValidationFailed.ToString()], Contains.Item("Вы не заполнили поле 'Login'"));
                Assert.That(details.Errors[ErrorCode.ValidationFailed.ToString()], Contains.Item("Вы не заполнили поле 'NewPassword'"));
            });
        }

        [Test]
        public async Task RecoveryAccessAsync_ValidationFailed_ReturnConflictLoginNotExist()
        {
            var apiRequest = CreateSampleRecoveryAccessUserApiRequest();

            var result = UserResult.FailureResult(ErrorCode.LoginNotExist);

            _recoveryAccessUserUseCaseMock.Setup(x => x.RecoveryAccessAsync(It.IsAny<RecoveryAccessUserCommand>())).ReturnsAsync(result);
            _defaultAuthenticateErrorMessageProviderMock.Setup(x => x.GetMessage(ErrorCode.LoginNotExist)).Returns("Нету логина");

            var actionResult = await _controller.RecoveryAccess(apiRequest);
            var conflictReauestResult = (ConflictObjectResult)actionResult;
            var errorResponseResult = (ErrorResponse)conflictReauestResult.Value!;

            Assert.Multiple(() =>
            {
                Assert.That(actionResult, Is.TypeOf<ConflictObjectResult>());
                Assert.That(conflictReauestResult.StatusCode, Is.EqualTo(409));
                Assert.That(errorResponseResult.ErrorCode, Is.EqualTo(ErrorCode.LoginNotExist.ToString()));
                Assert.That(errorResponseResult.Message, Is.EqualTo("Данный логин не существует."));
            });
        }

        [Test]
        public async Task RecoveryAccessAsync_ValidationFailed_ReturnConflictEmailNotExist()
        {
            var apiRequest = CreateSampleRecoveryAccessUserApiRequest();

            var result = UserResult.FailureResult(ErrorCode.EmailNotExist);

            _recoveryAccessUserUseCaseMock.Setup(x => x.RecoveryAccessAsync(It.IsAny<RecoveryAccessUserCommand>())).ReturnsAsync(result);
            _defaultAuthenticateErrorMessageProviderMock.Setup(x => x.GetMessage(ErrorCode.EmailNotExist)).Returns("Нету почты");

            var actionResult = await _controller.RecoveryAccess(apiRequest);
            var conflictReauestResult = (ConflictObjectResult)actionResult;
            var errorResponseResult = (ErrorResponse)conflictReauestResult.Value!;

            Assert.Multiple(() =>
            {
                Assert.That(actionResult, Is.TypeOf<ConflictObjectResult>());
                Assert.That(conflictReauestResult.StatusCode, Is.EqualTo(409));
                Assert.That(errorResponseResult.ErrorCode, Is.EqualTo(ErrorCode.EmailNotExist.ToString()));
                Assert.That(errorResponseResult.Message, Is.EqualTo("Данный почтовый адрес не существует."));
            });
        }

        [Test]
        public async Task RecoveryAccessAsync_ValidationFailed_ReturnObjectResultSaveUserError()
        {
            var apiRequest = CreateSampleRecoveryAccessUserApiRequest();

            var result = UserResult.FailureResult(ErrorCode.SaveUserError);

            _recoveryAccessUserUseCaseMock.Setup(x => x.RecoveryAccessAsync(It.IsAny<RecoveryAccessUserCommand>())).ReturnsAsync(result);

            var actionResult = await _controller.RecoveryAccess(apiRequest);
            var conflictReauestResult = (ObjectResult)actionResult;
            var errorResponseResult = (ErrorResponse)conflictReauestResult.Value!;

            Assert.Multiple(() =>
            {
                Assert.That(actionResult, Is.TypeOf<ObjectResult>());
                Assert.That(conflictReauestResult.StatusCode, Is.EqualTo(500));
                Assert.That(errorResponseResult.ErrorCode, Is.EqualTo(ErrorCode.SaveUserError.ToString()));
                Assert.That(errorResponseResult.Message, Is.EqualTo("Во время восстановления пароля произошла ошибка!!\nПожалуйста попробуйте позже."));
            });
        }

        [Test]
        public async Task RecoveryAccessAsync_ValidationFailed_ReturnObjectResultUnknownError()
        {
            var apiRequest = CreateSampleRecoveryAccessUserApiRequest();

            _recoveryAccessUserUseCaseMock.Setup(x => x.RecoveryAccessAsync(It.IsAny<RecoveryAccessUserCommand>())).Throws(new Exception());

            var actionResult = await _controller.RecoveryAccess(apiRequest);
            var conflictReauestResult = (ObjectResult)actionResult;
            var errorResponseResult = (ErrorResponse)conflictReauestResult.Value!;

            Assert.Multiple(() =>
            {
                Assert.That(actionResult, Is.TypeOf<ObjectResult>());
                Assert.That(conflictReauestResult.StatusCode, Is.EqualTo(500));
                Assert.That(errorResponseResult.ErrorCode, Is.EqualTo(ErrorCode.UnknownError.ToString()));
                Assert.That(errorResponseResult.Message, Is.EqualTo("Во время восстановления пароля произошла ошибка!!\nПожалуйста попробуйте позже. Или обратитесь в поддержку."));
            });
        }

        #endregion
    }
}