using System;
using System.Collections.Generic;

namespace ProAbono
{
    public class Pricing
    {
        public long IdSubscription { get; set; }
        public long IdFeature { get; set; }
        public bool? IsCustomerBillable { get; set; }
        public string LabelLocalized { get; set; }
        public string PricingLocalized { get; set; }
        public DateTime? DatePeriodStart { get; set; }
        public DateTime? DatePeriodTerm { get; set; }
        public MoveType TypeMove { get; set; }
        public int AmountSubtotal { get; set; }
        public int AmountTotal { get; set; }
        public List<Pricing> Details { get; set; }
        public Pricing NextTerm { get; set; }
    }
}