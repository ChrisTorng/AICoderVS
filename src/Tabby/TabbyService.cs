using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace AICoderVS.Tabby
{
    public class TabbyService
    {
        private JsonSerializerOptions options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        private readonly HttpClient _httpClient;
        private string _baseUrl;
        private string _bearerToken;

        public TabbyService()
        {
            LoadServiceConfig();

            var handler = new HttpClientHandler
            {
                UseProxy = true,
                Proxy = GetSystemWebProxy()
            };
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_bearerToken}");
        }

        private void LoadServiceConfig()
        {
            string json = File.ReadAllText("Service.json");
            var config = JsonSerializer.Deserialize<ServiceConfig>(json);
            _baseUrl = config.Server;
            _bearerToken = config.Token;
        }

        private IWebProxy GetSystemWebProxy()
        {
            var proxy = WebRequest.GetSystemWebProxy();
            proxy.Credentials = CredentialCache.DefaultCredentials;
            return proxy;
        }

        public async Task<HealthState> GetHealthStateAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("/v1/health");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<HealthState>(responseBody, options);
            }
            catch (HttpRequestException e)
            {
                MyLog.Log($"Error when sending request: {e.Message}");
                throw;
            }
            catch (JsonException e)
            {
                MyLog.Log($"Error when parsing response: {e.Message}");
                throw;
            }
        }
    }
}