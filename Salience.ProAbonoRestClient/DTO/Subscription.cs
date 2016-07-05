using System;
using System.Collections.Generic;

namespace ProAbono
{
    public class Subscription
    {
        public long Id { get; set; }
        public long IdSegment { get; set; }
        public long IdOffer { get; set; }
        public long IdCustomer { get; set; }
        public long IdCustomerBuyer { get; set; }
        public string ReferenceSegment { get; set; }
        public string ReferenceOffer { get; set; }
        public string ReferenceCustomer { get; set; }
        public string ReferenceCustomerBuyer { get; set; }
        public SubscriptionState StateSubscription { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DatePeriodStart { get; set; }
        public DateTime? DatePeriodEnd { get; set; }
        public DateTime? DateTerm { get; set; }
        public SubscriptionState StateSubscriptionAfterTerm { get; set; }
        public bool IsTrial { get; set; }
        public bool IsEngaged { get; set; }
        public bool IsCustomerBillable { get; set; }
        public bool IsPaymentCappingReached { get; set; }
        public DateTime? DateNextBilling { get; set; }
        public string TitleLocalized { get; set; }
        public string DescriptionLocalized { get; set; }
        public int DurationTrial { get; set; }
        public int AmountRecurrence { get; set; }
        public int? AmountUpFront { get; set; }
        public int? AmountTermination { get; set; }
        public int? AmountTrial { get; set; }
        public int? CountMinRecurrences { get; set; }
        public int? CountRecurrences { get; set; }
        public int? CountDaysTrial { get; set; }
        public int DurationRecurrence { get; set; }
        public TimeUnit UnitRecurrence { get; set; }
        public TimeUnit? UnitTrial { get; set; }
        public DateTime DateUpdate { get; set; }
        public long? IdUserUpdate { get; set; }

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