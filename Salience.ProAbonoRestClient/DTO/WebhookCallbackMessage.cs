using System;

namespace ProAbono
{
    public class WebhookCallbackMessage
    {
        public string Code { get; set; }
        public long Id { get; set; }
        public long IdBusiness { get; set; }
        public long IdWebhook { get; set; }
        public string ReferenceSegment { get; set; }
        public string Currency { get; set; }
        public DateTime DateTrigger { get; set; }
        public string TypeTrigger { get; set; }
        public WebhookCallbackMessageCustomer Customer { get; set; }
        public WebhookCallbackMessageCustomer CustomerBuyer { get; set; }
        public WebhookCallbackMessageOffer Offer { get; set; }
        public WebhookCallbackMessageSubscription Subscription { get; set; }
        public WebhookCallbackMessageInvoiceDebit InvoiceDebit { get; set; }
    }

    public class WebhookCallbackMessageCustomer : CustomerInfo
    {
        public long Id { get; set; }
        public string ReferenceCustomer { get; set; }
    }

    public class WebhookCallbackMessageOffer
    {
        public long Id { get; set; }
        public string ReferenceOffer { get; set; }
        public bool IsVisible { get; set; }
        public string Name { get; set; }
        public int AmountRecurrence { get; set; }
        public int DurationRecurrence { get; set; }
        public TimeUnit UnitRecurrence { get; set; }
    }

    public class WebhookCallbackMessageSubscription
    {
        public long Id { get; set; }
        public long IdSegment { get; set; }
        public string StateSubscription { get; set; }
        public DateTime DateStart { get; set; }
        public int AmountRecurrence { get; set; }
        public int DurationRecurrence { get; set; }
        public TimeUnit UnitRecurrence { get; set; }
    }

    public class WebhookCallbackMessageInvoiceDebit
    {
        public long Id { get; set; }
        public string FullNumber { get; set; }
        public string StateInvoice { get; set; }
        public DateTime? DateIssue { get; set; }
        public DateTime? DatePayment { get; set; }
        public string TypePayment { get; set; }
        public int AmountSubtotal { get; set; }
        public int AmountTotal { get; set; }
    }
}
