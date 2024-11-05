using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using DataModels.Utilities;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

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
        var responseJson = await response.Content.ReadAsStringAsync();
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
}
