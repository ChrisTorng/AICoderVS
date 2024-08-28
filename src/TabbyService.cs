using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;

namespace AICoderVS
{
    public class TabbyService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://tabby-chris-torng-8a8dded8.mt-guc1.bentoml.ai";

        public TabbyService()
        {
            var handler = new HttpClientHandler
            {
                UseProxy = true,
                Proxy = GetSystemWebProxy()
            };
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(BaseUrl);
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
                return JsonSerializer.Deserialize<HealthState>(responseBody);
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

    public class HealthState
    {
        public string Model { get; set; }
        public string ChatModel { get; set; }
        public string ChatDevice { get; set; }
        public string Device { get; set; }
        public string Arch { get; set; }
        public string CpuInfo { get; set; }
        public int CpuCount { get; set; }
        public string[] CudaDevices { get; set; }
        public Version Version { get; set; }
        public bool? Webserver { get; set; }
    }

    public class Version
    {
        public string BuildDate { get; set; }
        public string BuildTimestamp { get; set; }
        public string GitSha { get; set; }
        public string GitDescribe { get; set; }
    }
}