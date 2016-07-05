using System;
using System.Collections.Generic;

namespace ProAbono
{
    public class Subscription
    {
        public int AmountRecurrence { get; set; }
        public int? AmountUpFront { get; set; }
        public int? AmountTermination { get; set; }
        public int? AmountTrial { get; set; }
        public int? CountMinRecurrences { get; set; }
        public int? CountRecurrences { get; set; }
        public DateTime DateUpdate { get; set; }
        public string DescriptionLocalized { get; set; }
        public int DurationRecurrence { get; set; }
        public int? DurationTrial { get; set; }
        public long Id { get; set; }
        public long IdCustomer { get; set; }
        public long IdCustomerBuyer { get; set; }
        public long IdOffer { get; set; }
        public long IdSegment { get; set; }
        public long? IdUserUpdate { get; set; }
        public string ReferenceCustomer { get; set; }
        public string ReferenceCustomerBuyer { get; set; }
        public string ReferenceOffer { get; set; }
        public string ReferenceSegment { get; set; }
        public SubscriptionState StateSubscription { get; set; }
        public string TitleLocalized { get; set; }
        public TimeUnit UnitRecurrence { get; set; }
        public TimeUnit? UnitTrial { get; set; }
    }

    public class DetailedSubscription : Subscription
    {
        public DateTime? DateStart { get; set; }
        public DateTime? DateDeadline { get; set; }
        public DateTime? DateRenewal { get; set; }
        public DateTime? DatePeriodStart { get; set; }
        public DateTime? DatePeriodEnd { get; set; }
        public int CountDaysTrial { get; set; }
        public List<SubscriptionFeature> Features { get; set; }
        public List<Link> Links { get; set; }
    }

    public class SubscriptionFeature : FeatureCharacteristics
    {
        public long Id { get; set; }
        public string DescriptionLocalized { get; set; }
        public string TitleLocalized { get; set; }
    }
}