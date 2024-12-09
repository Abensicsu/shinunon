using System.Text;
using DataModels.Utilities;
using Newtonsoft.Json;
using ShWeb.Components.BAServices;

public class CustomHttpClientService
{
    private readonly HttpClient _httpClient;
    private readonly LocalStorageService _localStorageService;

    public CustomHttpClientService(HttpClient httpClient, LocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _localStorageService = localStorageService;
    }

    private async Task SetAuthorizationHeaderAsync()
    {
        var token = await _localStorageService.GetFromLocalStorageAsync<string>("jwt_token");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }


    public async Task<T> GetAsync<T>(string requestUri)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.GetStringAsync(requestUri);
        var settings = JsonSerializerConfig.GetSettings(); // Use configured settings
        return JsonConvert.DeserializeObject<T>(response, settings);
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest request)
    {
        await SetAuthorizationHeaderAsync();

        var settings = JsonSerializerConfig.GetSettings(); // Use configured settings

        var json = JsonConvert.SerializeObject(request, settings);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(requestUri, content);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(errorMessage);
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TResponse>(responseJson, settings);
    }

    public async Task<TResponse> PutAsync<TRequest, TResponse>(string requestUri, TRequest request)
    {
        await SetAuthorizationHeaderAsync();

        var settings = JsonSerializerConfig.GetSettings(); // Use configured settings

        var json = JsonConvert.SerializeObject(request, settings);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync(requestUri, content);
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TResponse>(responseJson, settings);
    }

    public async Task<bool> DeleteAsync(string requestUri)
    {
        await SetAuthorizationHeaderAsync();

        var response = await _httpClient.DeleteAsync(requestUri);
        return response.IsSuccessStatusCode;
    }

    public class TokenResponse
    {
        public string Token { get; set; }
    }
}
