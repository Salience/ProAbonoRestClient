using System;

namespace ProAbono
{
    public class FeatureCharacteristics
    {
        public string ReferenceFeature { get; set; }
        public FeatureType TypeFeature { get; set; }
        public bool IsVisible { get; set; }

        public bool? IsEnabled { get; set; }
        public bool? IsIncluded { get; set; }
        public int? QuantityCurrent { get; set; }
        public int? QuantityIncluded { get; set; }
    }

    public class Feature : FeatureCharacteristics
    {
        public long Id { get; set; }
        public string DescriptionLocalized { get; set; }
        public string TitleLocalized { get; set; }
        public string PricingLocalized { get; set; }
        public string Properties { get; set; }
        public DateTime? DatePeriodEnd { get; set; }
        public DateTime? DatePeriodStart { get; set; }
    }
}