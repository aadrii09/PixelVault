using Stripe;
using ApiVault.Settings;
using ApiVault.DTOs;

namespace ApiVault.Services
{
    public class StripeService
    {
        private readonly StripeSettings _stripeSettings;

        public StripeService(StripeSettings stripeSettings)
        {
            _stripeSettings = stripeSettings;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        }

        public async Task<string> CreatePaymentIntentAsync(decimal amount)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100), // por los centavos
                Currency = "eur",
                PaymentMethodTypes = new List<string> { "card" }
            };

            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);

            return intent.ClientSecret;
        }

        public async Task<bool> VerifyPaymentAsync(string paymentIntentId)
        {
            var service = new PaymentIntentService();
            var intent = await service.GetAsync(paymentIntentId);

            return intent.Status == "succeeded";
        }
    }
}