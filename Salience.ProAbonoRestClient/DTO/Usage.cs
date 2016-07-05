using System;

namespace ProAbono
{
    public class Usage : FeatureCharacteristics
    {
        public long IdSegment { get; set; }
        public long IdFeature { get; set; }
        public long IdCustomer { get; set; }
        public long IdSubscription { get; set; }
        public string ReferenceSegment { get; set; }
        public string ReferenceCustomer { get; set; }
        public DateTime? DatePeriodEnd { get; set; }
        public DateTime? DatePeriodStart { get; set; }
    }
}