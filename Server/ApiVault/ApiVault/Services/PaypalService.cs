using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ApiVault.DTOs;
using ApiVault.Settings;
using Microsoft.Extensions.Options;

namespace ApiVault.Services
{
    public class PaypalService
    {
        private readonly HttpClient _httpClient;
        private readonly PaypalSettings _settings;

        public PaypalService(IOptions<PaypalSettings> settings)
        {
            _httpClient = new HttpClient();
            _settings = settings.Value;
        }

        public async Task<string> ObtenerTokenAsync()
        {
            var auth = Encoding.UTF8.GetBytes($"{_settings.ClientId}:{_settings.Secret}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(auth));


            var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await _httpClient.PostAsync($"{_settings.BaseUrl}/v1/oauth2/token", content);
            var result = await response.Content.ReadAsStringAsync();

            using var jsonDoc = JsonDocument.Parse(result);
            return jsonDoc.RootElement.GetProperty("access_token").GetString();
        }

        public async Task<string> CrearOrderAsync(MontoDto dto)
        {
            var token = await ObtenerTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var body = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                    new
                    {
                        amount = new
                        {
                            currency_code = dto.Currency,
                            value = dto.Total.ToString("F2")
                        }
                    }
                }
            };
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_settings.BaseUrl}/v2/checkout/orders", content);
            var result = await response.Content.ReadAsStringAsync();

            using var jsonDoc = JsonDocument.Parse(result);
            var orderId = jsonDoc.RootElement.GetProperty("id").GetString();
            return orderId;


        }
    }
}
