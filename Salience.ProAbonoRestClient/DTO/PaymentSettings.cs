using System;

namespace ProAbono
{
    public class PaymentSettings
    {
        public PaymentType? TypePayment { get; set; }
        public DateTime? DateNextBilling { get; set; }
    }
}