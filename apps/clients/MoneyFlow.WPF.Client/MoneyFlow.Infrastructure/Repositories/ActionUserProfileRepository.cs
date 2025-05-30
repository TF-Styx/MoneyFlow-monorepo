using MoneyFlow.Application.InterfaceRepositories;
using MoneyFlow.Domain.DomainModels;
using MoneyFlow.Domain.Enums;
using MoneyFlow.Domain.Results;
using MoneyFlow.Infrastructure.NetworksModels.Request;
using MoneyFlow.Infrastructure.NetworksModels.Response;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace MoneyFlow.Infrastructure.Repositories
{
    public class ActionUserProfileRepository : IActionUserProfileRepository
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;

        public ActionUserProfileRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task<Result<UserDomain>> AuthenticateAsync(string login, string password)
        {
            string url = "api/users/authenticate";

            var userRequest = new AuthRequest { Login = login, Password = password };

            var apiResponse = await _httpClient.PostAsJsonAsync(url, userRequest, _serializerOptions);

            if (apiResponse.StatusCode == HttpStatusCode.OK)
            {
                var userResponse = await apiResponse.Content.ReadFromJsonAsync<UserResponse>(_serializerOptions);

                var domain = UserDomain.Reconstitute(userResponse!.IdUser, userResponse.Login, userResponse.UserName, userResponse.Email, userResponse.Phone, userResponse.IdGender);

                return Result<UserDomain>.SuccessResult(domain);
            }
            else
            {
                string error = await apiResponse.Content.ReadAsStringAsync();

                return Result<UserDomain>.FailureResult(new ErrorDetails(ErrorCode.AuthenticateError, $"Ошибка: {error}!!\nКод ошибки: {apiResponse.StatusCode}"));
            }
        }

        public async Task<Result<(string Login, string Password)?>> RegistrationAsync(string userName, string email, string login, string password, int idGender, string? phone)
        {
            string url = "api/users/register";

            var userRequest = new RegisterRequest()
            {
                Login = login,
                Password = password,
                UserName = userName,
                Email = email,
                Phone = phone,
                IdGender = idGender,
            };

            var apiResponse = await _httpClient.PostAsJsonAsync(url, userRequest, _serializerOptions);

            if (apiResponse.StatusCode == HttpStatusCode.Created)
            {
                return Result<ValueTuple<string, string>?>.SuccessResult((login, password));
            }
            else
            {
                var error = apiResponse.Content.ReadAsStringAsync();
                return Result<ValueTuple<string, string>?>.FailureResult(new ErrorDetails(ErrorCode.RegisterApiError, $"Ошибка: {error}. {apiResponse.StatusCode}"));
            }
        }

        public async Task<Result<(string Login, string NewPassword)?>> RecoveryAccessAsync(string email, string login, string newPassword)
        {
            string url = "api/users/recovery-access";

            var userRequest = new RecoveryAccessRequest()
            {
                Email = email,
                Login = login,
                NewPassword = newPassword
            };

            var apiResponse = await _httpClient.PostAsJsonAsync(url, userRequest, _serializerOptions);

            if (apiResponse.StatusCode == HttpStatusCode.OK)
            {
                return Result<ValueTuple<string, string>?>.SuccessResult((login, newPassword));
            }
            else
            {
                var error = apiResponse.Content.ReadAsStringAsync();
                return Result<ValueTuple<string, string>?>.FailureResult(new ErrorDetails(ErrorCode.RecoveryApiError, $"Ошибка: {error}. {apiResponse.StatusCode}"));
            }
        }
    }
}
