using System;

namespace ProAbono
{
    public class WebhookCallbackMessage
    {
        public string Code { get; set; }
        public int Id { get; set; }
        public int IdBusiness { get; set; }
        public int IdWebhook { get; set; }
        public string ReferenceSegment { get; set; }
        public string Currency { get; set; }
        public DateTime DateTrigger { get; set; }
        public string TypeTrigger { get; set; }
        public WebhookCallbackMessageCustomer Customer { get; set; }
        public WebhookCallbackMessageCustomer CustomerBuyer { get; set; }
        public WebhookCallbackMessageOffer Offer { get; set; }
        public WebhookCallbackMessageSubscription Subscription { get; set; }
    }

    public class WebhookCallbackMessageCustomer : CustomerInfo
    {
        public int Id { get; set; }
        public string ReferenceCustomer { get; set; }
    }

    public class WebhookCallbackMessageOffer
    {
        public int Id { get; set; }
        public string ReferenceOffer { get; set; }
        public bool IsVisible { get; set; }
        public string Name { get; set; }
        public int AmountRecurrence { get; set; }
        public int DurationRecurrence { get; set; }
        public TimeUnit UnitRecurrence { get; set; }
    }

    public class WebhookCallbackMessageSubscription
    {
        public int Id { get; set; }
        public int IdSegment { get; set; }
        public string StateSubscription { get; set; }
        public DateTime DateStart { get; set; }
        public int AmountRecurrence { get; set; }
        public int DurationRecurrence { get; set; }
        public TimeUnit UnitRecurrence { get; set; }
    }
}
