using MoneyFlow.Application.InterfaceRepositories;
using MoneyFlow.Domain.DomainModels;
using MoneyFlow.Domain.Enums;
using MoneyFlow.Domain.Results;
using MoneyFlow.Infrastructure.NetworksModels.Response;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MoneyFlow.Infrastructure.Repositories
{
    public class GenderRepository : IGenderRepository
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;

        public GenderRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task<Result<List<GenderDomain>>> GetAllAsync()
        {
            try
            {
                string url = "api/genders/get-all";

                var apiResponse = await _httpClient.GetAsync(url);

                if (apiResponse.IsSuccessStatusCode)
                {
                    var genderResponse = await apiResponse.Content.ReadFromJsonAsync<List<GenderResponse>>(_serializerOptions);

                    var domain = genderResponse!.Select(x => GenderDomain.Reconstitute(x.IdGender, x.GenderName)).ToList();

                    return Result<List<GenderDomain>>.SuccessResult(domain!);
                }
                else
                {
                    string error = await apiResponse.Content.ReadAsStringAsync();

                    return Result<List<GenderDomain>>.FailureResult(new ErrorDetails(ErrorCode.ValueEmpty, $"Ошибка: {error}!!\nКод ошибка: {apiResponse.StatusCode}"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
