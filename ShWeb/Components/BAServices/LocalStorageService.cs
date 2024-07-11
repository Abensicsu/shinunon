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
            var settings = GetJsonSerializerSettings();
            var json = JsonConvert.SerializeObject(value, settings);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
        }

        public async Task<T> GetFromLocalStorageAsync<T>(string key)
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            if (json == null) return default(T);
            var settings = GetJsonSerializerSettings();
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        private JsonSerializerSettings GetJsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

         public async Task RemoveFromLocalStorageAsync(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
}
