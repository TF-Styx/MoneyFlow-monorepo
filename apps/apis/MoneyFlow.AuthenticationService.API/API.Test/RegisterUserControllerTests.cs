using Microsoft.AspNetCore.Mvc;
using MoneyFlow.AuthenticationService.API.Controllers;
using MoneyFlow.AuthenticationService.API.DTOs.Request;
using MoneyFlow.AuthenticationService.Application.DTOs;
using MoneyFlow.AuthenticationService.Application.DTOs.Commands;
using MoneyFlow.AuthenticationService.Application.DTOs.Requests.Response;
using MoneyFlow.AuthenticationService.Application.DTOs.Results;
using MoneyFlow.AuthenticationService.Application.Enums;
using MoneyFlow.AuthenticationService.Application.Providers.Abstraction;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.UserUseCases;
using Moq;

namespace API.Test
{
    [TestFixture]
    internal class RegisterUserControllerTests
    {
        private Mock<ICreateUserUseCase> _createUserUseCaseMock;
        private Mock<IDefaultRegistrationErrorMessageProvider> _defaultRegistrationErrorMessageProviderMock;
        private RegisterUserController _controller;

        [SetUp]
        public void SetUp()
        {
            _createUserUseCaseMock = new Mock<ICreateUserUseCase>();
            _defaultRegistrationErrorMessageProviderMock = new Mock<IDefaultRegistrationErrorMessageProvider>();
            _controller = new RegisterUserController(_createUserUseCaseMock.Object, _defaultRegistrationErrorMessageProviderMock.Object);
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

        [Test]
        public async Task CreateAsync_ValidRequest_ReturnCreateStatus201WithUser()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest();

            var userDTO = CreateSampleUserDTO();
            var result = RegisterUserResult.SuccessResult(userDTO);

            _createUserUseCaseMock.Setup(x => x.CreateAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);

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
        public async Task CreateAsync_ValidationFailed_ReturnBadRequestWithValidationErrors()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest(login: " ");

            var validationErrors = new List<string> { "Вы не заполнили поле 'Login'!!", "Пароль не достаточно надежен!!" };
            var result = RegisterUserResult.ValidationFailureResult(validationErrors, "Ошибка валидации!!");

            _createUserUseCaseMock.Setup(x => x.CreateAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(RegistrationErrorCode.ValidationFailed)).Returns("Ошибка валидации данных!!");

            var actionResult = await _controller.Register(apiRequest);
            var badRequestResult = (BadRequestObjectResult)actionResult;
            var problemDetails = (ValidationProblemDetails)badRequestResult.Value!;

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(problemDetails.Errors[RegistrationErrorCode.ValidationFailed.ToString()], Contains.Item("Вы не заполнили поле 'Login'!!"));
            Assert.That(problemDetails.Errors[RegistrationErrorCode.ValidationFailed.ToString()], Contains.Item("Пароль не достаточно надежен!!"));
        }

        [Test]
        public async Task CreateAsync_ValidationFailed_ReturnBadRequestWithErrorMessage()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest();

            var result = RegisterUserResult.FailureResult(RegistrationErrorCode.ValidationFailed, "Ошибка валидации!!");

            _createUserUseCaseMock.Setup(x => x.CreateAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(RegistrationErrorCode.ValidationFailed)).Returns("Ошибка валидации данных!!");

            var actionResult = await _controller.Register(apiRequest);
            var badRequestResult = (BadRequestObjectResult)actionResult;
            var errorResponse = (ErrorResponse)badRequestResult.Value!;

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(errorResponse, Is.TypeOf<ErrorResponse>());
            Assert.That(errorResponse.ErrorCode, Is.EqualTo(RegistrationErrorCode.ValidationFailed.ToString()));
            Assert.That(errorResponse.Message, Is.EqualTo("Ошибка валидации!!"));
        }

        [Test]
        public async Task CreateAsync_LoginAlreadyTaken_ReturnConflict()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest(login: "TestExistLogin");
            
            var result = RegisterUserResult.FailureResult(RegistrationErrorCode.LoginAlreadyTaken, $"Логин: '{apiRequest.Login}' уже занят, придумайте новый!!");

            _createUserUseCaseMock.Setup(x => x.CreateAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(RegistrationErrorCode.LoginAlreadyTaken)).Returns("Этот логин уже занят");

            var actionResult = await _controller.Register(apiRequest);
            var conflictRequestResult = (ConflictObjectResult)actionResult;
            var resultValue = (ErrorResponse)conflictRequestResult.Value!;

            TestContext.Out.WriteLine(conflictRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<ConflictObjectResult>());
            Assert.That(conflictRequestResult.StatusCode, Is.EqualTo(409));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(RegistrationErrorCode.LoginAlreadyTaken.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo($"Логин: '{apiRequest.Login}' уже занят, придумайте новый!!"));
        }

        [Test]
        public async Task CreateAsync_EmailAlreadyRegistered_ReturnConflict()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest();
            
            var result = RegisterUserResult.FailureResult(RegistrationErrorCode.EmailAlreadyRegistered, $"'Email': '{apiRequest.Email}' уже занят!!");

            _createUserUseCaseMock.Setup(x => x.CreateAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(RegistrationErrorCode.EmailAlreadyRegistered)).Returns("Этот почтовый адрес уже занят");

            var actionResult = await _controller.Register(apiRequest);
            var conflictRequestResult = (ConflictObjectResult)actionResult;
            var resultValue = (ErrorResponse)conflictRequestResult.Value!;

            TestContext.Out.WriteLine(conflictRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<ConflictObjectResult>());
            Assert.That(conflictRequestResult.StatusCode, Is.EqualTo(409));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(RegistrationErrorCode.EmailAlreadyRegistered.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo($"'Email': '{apiRequest.Email}' уже занят!!"));
        }

        [Test]
        public async Task CreateAsync_PhoneAlreadyRegistered_ReturnConflict()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest();
            
            var result = RegisterUserResult.FailureResult(RegistrationErrorCode.PhoneAlreadyRegistered, $"Данный 'Phone': '{apiRequest.Phone}' уже занят!!");

            _createUserUseCaseMock.Setup(x => x.CreateAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(RegistrationErrorCode.PhoneAlreadyRegistered)).Returns("Этот номер телефона уже зарегистрирован");

            var actionResult = await _controller.Register(apiRequest);
            var conflictRequestResult = (ConflictObjectResult)actionResult;
            var resultValue = (ErrorResponse)conflictRequestResult.Value!;

            TestContext.Out.WriteLine(conflictRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<ConflictObjectResult>());
            Assert.That(conflictRequestResult.StatusCode, Is.EqualTo(409));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(RegistrationErrorCode.PhoneAlreadyRegistered.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo($"Данный 'Phone': '{apiRequest.Phone}' уже занят!!"));
        }

        [Test]
        public async Task CreateAsync_WeakPassword_ReturnBadRequest()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest();

            var result = RegisterUserResult.FailureResult(RegistrationErrorCode.WeakPassword, $"Ваш пароль слишком легкий");

            _createUserUseCaseMock.Setup(x => x.CreateAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(RegistrationErrorCode.WeakPassword)).Returns("Недостаточная безопасность пароля!!");

            var actionResult = await _controller.Register(apiRequest);
            var badRequestResult = (BadRequestObjectResult)actionResult;
            var resultValue = (ErrorResponse)badRequestResult.Value!;

            TestContext.Out.WriteLine(badRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(RegistrationErrorCode.WeakPassword.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo("Ваш пароль слишком легкий"));
        }

        [Test]
        public async Task CreateAsync_InvalidRole_ReturnBadRequest()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest();

            var result = RegisterUserResult.FailureResult(RegistrationErrorCode.InvalidRole, "Не указана роль");

            _createUserUseCaseMock.Setup(x => x.CreateAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(RegistrationErrorCode.InvalidRole)).Returns("Не назначена роль!!");

            var actionResult = await _controller.Register(apiRequest);
            var badRequestResult = (BadRequestObjectResult)actionResult;
            var resultValue = (ErrorResponse)badRequestResult.Value!;

            TestContext.Out.WriteLine(badRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(RegistrationErrorCode.InvalidRole.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo("Не указана роль"));
        }

        [Test]
        public async Task CreateAsync_DomainCreationError_ReturnBadRequest()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest();

            var result = RegisterUserResult.FailureResult(RegistrationErrorCode.DomainCreationError, "Не создана DomainModel");

            _createUserUseCaseMock.Setup(x => x.CreateAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(RegistrationErrorCode.DomainCreationError)).Returns("Не удалось создать DomainModel!!");

            var actionResult = await _controller.Register(apiRequest);
            var badRequestResult = (BadRequestObjectResult)actionResult;
            var resultValue = (ErrorResponse)badRequestResult.Value!;

            TestContext.Out.WriteLine(badRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(RegistrationErrorCode.DomainCreationError.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo("Не создана DomainModel"));
        }

        [Test]
        public async Task CreateAsync_SaveUserError_ReturnBadRequest()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest();

            var result = RegisterUserResult.FailureResult(RegistrationErrorCode.SaveUserError);

            _createUserUseCaseMock.Setup(x => x.CreateAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(RegistrationErrorCode.SaveUserError)).Returns("Не удалось сохранить пользователя!!");

            var actionResult = await _controller.Register(apiRequest);
            var badRequestResult = (ObjectResult)actionResult;
            var resultValue = (ErrorResponse)badRequestResult.Value!;

            TestContext.Out.WriteLine(badRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<ObjectResult>());
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(500));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(RegistrationErrorCode.SaveUserError.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo("Во время регистрации произошла ошибка!!\nПожалуйста попробуйте позже."));
        }

        [Test]
        public async Task CreateAsync_UnknownError_ReturnBadRequest()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest();

            var result = RegisterUserResult.FailureResult(RegistrationErrorCode.UnknownError);

            _createUserUseCaseMock.Setup(x => x.CreateAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(RegistrationErrorCode.UnknownError)).Returns("Неизвестная ошибка!!");

            var actionResult = await _controller.Register(apiRequest);
            var badRequestResult = (ObjectResult)actionResult;
            var resultValue = (ErrorResponse)badRequestResult.Value!;

            TestContext.Out.WriteLine(badRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<ObjectResult>());
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(500));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(RegistrationErrorCode.UnknownError.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo("Во время регистрации произошла ошибка!!\nПожалуйста попробуйте позже."));
        }

        [Test]
        public async Task CreateAsync_None_ReturnBadRequest()
        {
            var apiRequest = CreateSampleRegisterUserApiRequest();

            var result = RegisterUserResult.FailureResult(RegistrationErrorCode.None);

            _createUserUseCaseMock.Setup(x => x.CreateAsync(It.IsAny<RegisterUserCommand>())).ReturnsAsync(result);
            _defaultRegistrationErrorMessageProviderMock.Setup(x => x.GetMessage(RegistrationErrorCode.None)).Returns("Неизвестная ошибка!!");

            var actionResult = await _controller.Register(apiRequest);
            var badRequestResult = (ObjectResult)actionResult;
            var resultValue = (ErrorResponse)badRequestResult.Value!;

            TestContext.Out.WriteLine(badRequestResult.Value);

            Assert.That(actionResult, Is.TypeOf<ObjectResult>());
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(500));
            Assert.That(resultValue, Is.TypeOf<ErrorResponse>());
            Assert.That(resultValue.ErrorCode, Is.EqualTo(RegistrationErrorCode.None.ToString()));
            Assert.That(resultValue.Message, Is.EqualTo("Во время регистрации произошла ошибка!!\nПожалуйста попробуйте позже."));
        }
    }
}