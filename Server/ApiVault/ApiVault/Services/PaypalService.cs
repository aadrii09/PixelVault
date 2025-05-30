using System.Globalization;
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

            Console.WriteLine($"Recibida solicitud para crear orden PayPal con total: {dto.Total} {dto.Currency}");

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
                    value = dto.Total.ToString("F2", CultureInfo.InvariantCulture)
                }
            }
        }
            };

            var json = JsonSerializer.Serialize(body);
            Console.WriteLine("🟡 Payload a PayPal:");
            Console.WriteLine(json);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_settings.BaseUrl}/v2/checkout/orders", content);
            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("🟢 Respuesta de PayPal:");
            Console.WriteLine(result);

            response.EnsureSuccessStatusCode();

            using var jsonDoc = JsonDocument.Parse(result);
            return jsonDoc.RootElement.GetProperty("id").GetString();
        }


        public async Task<bool> VerificarOrdenAsync(string orderId)
        {
            Console.WriteLine($"🔍 Verificando y capturando orden PayPal: {orderId}");

            var token = await ObtenerTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Capturar la orden (para confirmar el pago)
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_settings.BaseUrl}/v2/checkout/orders/{orderId}/capture");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent("{}", Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);


            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"🟢 Respuesta CAPTURE PayPal:");
            Console.WriteLine(result);

            response.EnsureSuccessStatusCode();

            using var jsonDoc = JsonDocument.Parse(result);
            var status = jsonDoc.RootElement.GetProperty("status").GetString();

            Console.WriteLine($"📦 Estado después del capture: {status}");

            return status == "COMPLETED";
        }
    }
}