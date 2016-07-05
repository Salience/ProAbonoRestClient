using System;
using System.Collections.Generic;

namespace ProAbono
{
    public class CustomerInfo
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
    }

    public class Customer : CustomerInfo
    {
        public long Id { get; set; }
        public long IdSegment { get; set; }
        public string ReferenceCustomer { get; set; }
        public string ReferenceSegment { get; set; }
        public List<Link> Links { get; set; }
    }

    public class CustomerWithUsage : Customer
    {
        public int? QuantityIncluded { get; set; }
        public int? QuantityCurrent { get; set; }
        public bool? IsIncluded { get; set; }
        public bool? IsEnabled { get; set; }
        public DateTime? DatePeriodStart { get; set; }
        public DateTime? DatePeriodEnd { get; set; }
    }
}