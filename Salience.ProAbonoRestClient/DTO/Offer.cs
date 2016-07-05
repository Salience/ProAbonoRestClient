using System;
using System.Collections.Generic;

namespace ProAbono
{
    public class Offer
    {
        public int AmountRecurrence { get; set; }
        public int? AmountUpFront { get; set; }
        public int? AmountTermination { get; set; }
        public int? AmountTrial { get; set; }
        public int? CountMinRecurrences { get; set; }
        public int? CountRecurrences { get; set; }
        public string Currency { get; set; }
        public string DescriptionLocalized { get; set; }
        public int DurationRecurrence { get; set; }
        public int? DurationTrial { get; set; }
        public List<Feature> Features { get; set; }
        public long Id { get; set; }
        public long IdSegment { get; set; }
        public bool IsVisible { get; set; }
        public List<Link> Links { get; set; }
        public string Name { get; set; }
        public string PricingLocalized { get; set; }
        public string ReferenceOffer { get; set; }
        public string ReferenceSegment { get; set; }
        public string TitleLocalized { get; set; }
        public TimeUnit UnitRecurrence { get; set; }
        public TimeUnit? UnitTrial { get; set; }
    }

    public class SubscribableOffer : Offer
    {
        public long? IdSubscription { get; set; }
        public DateTime? DateSubscription { get; set; }
    }
}