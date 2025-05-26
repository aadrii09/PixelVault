namespace ApiVault.DTOs
{
    public class PaymentIntentDto
    {
        public decimal Amount { get; set; }
    }

    public class VerifyPaymentDto
    {
        public string PaymentIntentId { get; set; }
    }
}