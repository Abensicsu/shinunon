using DataModels.Utilities;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ShWeb.Components.BAServices
{
    public class LocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task StoreInLocalStorageAsync<T>(string key, T value)
        {
            var settings = JsonSerializerConfig.GetSettings(); // Use configured settings

            var json = JsonConvert.SerializeObject(value, settings);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
        }

        public async Task<T> GetFromLocalStorageAsync<T>(string key)
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            if (json == null) return default(T);
            var settings = JsonSerializerConfig.GetSettings(); // Use configured settings

            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        public async Task RemoveFromLocalStorageAsync(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
}
