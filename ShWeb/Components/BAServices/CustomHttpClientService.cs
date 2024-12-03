using System.Text;
using DataModels.Utilities;
using Newtonsoft.Json;

public class CustomHttpClientService
{
    private readonly HttpClient _httpClient;

    public CustomHttpClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T> GetAsync<T>(string requestUri)
    {
        var response = await _httpClient.GetStringAsync(requestUri);
        var settings = JsonSerializerConfig.GetSettings(); // Use configured settings
        return JsonConvert.DeserializeObject<T>(response, settings);
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest request)
    {
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
        Console.WriteLine($"Response JSON: {responseJson}");
        return JsonConvert.DeserializeObject<TResponse>(responseJson, settings);
    }

    public async Task<TResponse> PutAsync<TRequest, TResponse>(string requestUri, TRequest request)
    {
        var settings = JsonSerializerConfig.GetSettings(); // Use configured settings

        var json = JsonConvert.SerializeObject(request, settings);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync(requestUri, content);
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TResponse>(responseJson, settings);
    }

    public async Task<bool> DeleteAsync(string requestUri)
    {
        var response = await _httpClient.DeleteAsync(requestUri);
        return response.IsSuccessStatusCode;
    }

    public class TokenResponse
{
    public string Token { get; set; }
}

}
