using System;
using Salience.FluentApi;

namespace ProAbono
{
    //public interface IProAbonoClient
    //{
    //    #region Features
    //    IExecutableRequest<Feature> RetrieveFeature(string referenceFeature, string referenceSegment, string language = null, bool? html = true);
    //    IExecutableRequest<Feature> RetrieveFeatureForCustomer(string referenceFeature, string referenceCustomer, string language = null, bool? html = true);
    //    IExecutableRequest<PaginatedList<Feature>> RetrieveFeatures(string referenceSegment, bool? isVisible = null, string language = null, bool? html = true, int? page = null, int? sizePage = null);
    //    IExecutableRequest<PaginatedList<Feature>> RetrieveFeaturesForCustomer(string referenceCustomer, bool? isVisible = null, bool? html = true, int? page = null, int? sizePage = null);
    //    #endregion

    //    #region Customers
    //    IExecutableRequest<Customer> CreateCustomer(string referenceSegment, string referenceCustomer, CustomerInfo customerInfo);
    //    IExecutableRequest<Customer> RetrieveCustomer(string referenceCustomer);
    //    IExecutableRequest<Customer> UpdateCustomer(string referenceCustomer, CustomerInfo customerInfo);
    //    IExecutableRequest<PaginatedList<Customer>> ListCustomers(string referenceSegment, int? page = null, int? sizePage = null);
    //    IExecutableRequest<PaginatedList<CustomerWithUsage>> ListCustomersByFeature(string referenceFeature, string referenceSegment, int? page = null, int? sizePage = null);
    //    IExecutableRequest<Address> RetrieveBillingAddress(string referenceCustomer);
    //    IExecutableRequest<Address> UpdateBillingAddress(string referenceCustomer, Address billingAddress);
    //    IExecutableRequest<Address> RetrieveShippingAddress(string referenceCustomer);
    //    IExecutableRequest<Address> UpdateShippingAddress(string referenceCustomer, Address shippingAddress);
    //    #endregion 
       
    //    #region Usages
    //    IExecutableRequest<PaginatedList<Usage>> RetrieveUsagesForCustomer(string referenceCustomer);
    //    IExecutableRequest<PaginatedList<Usage>> RetrieveUsagesForFeature(string referenceFeature);
    //    IExecutableRequest<Usage> RetrieveUsageForCustomer(string referenceCustomer, string referenceFeature);
    //    IExecutableRequest<Usage> UpdateUsageByIncrement(string referenceCustomer, string referenceFeature, int increment, DateTime dateStamp, long? idSubscription = null);
    //    IExecutableRequest<Usage> UpdateUsageByCurrentQuantity(string referenceCustomer, string referenceFeature, int quantityCurrent, DateTime dateStamp, long? idSubscription = null);
    //    IExecutableRequest<Usage> UpdateUsageOnOffFeature(string referenceCustomer, string referenceFeature, bool isEnabled, DateTime dateStamp, long? idSubscription = null);
    //    IExecutableRequest<Pricing> EstimatePricingOfUsageUpdate(string referenceCustomer, string referenceFeature, int increment, DateTime dateStamp, long? idSubscription = null);
    //    #endregion        

    //    #region Offers
    //    IExecutableRequest<Offer> RetrieveOffer(string referenceSegment = null, string referenceOffer = null, string language = null, bool? html = true, bool? ignoreFeatures = false, bool? links = false);
    //    IExecutableRequest<Offer> RetrieveOfferForCustomer(string referenceCustomer, string referenceOffer = null, string language = null, bool? html = true, bool? ignoreFeatures = false, bool? links = false);
    //    IExecutableRequest<PaginatedList<Offer>> RetrieveOffers(string referenceSegment = null, string language = null, bool? html = true, bool? isVisible = null, bool? ignoreFeatures = false, bool? links = false, int? page = null, int? sizePage = null);
    //    IExecutableRequest<PaginatedList<Offer>> RetrieveOffersForCustomer(string referenceCustomer, bool? html = true, bool? isVisible = null, bool? ignoreFeatures = false, bool? links = false, int? page = null, int? sizePage = null);
    //    IExecutableRequest<PaginatedList<Offer>> RetrieveOffersToUpgradeCustomer(string referenceCustomer, bool upgrade, bool? html = true, bool? isVisible = null, bool? ignoreFeatures = false, bool? links = false, int? page = null, int? sizePage = null);
    //    IExecutableRequest<PaginatedList<Offer>> RetrieveOffersToUpgradeCustomer(long idSubscription, bool? html = true, bool? isVisible = null, bool? ignoreFeatures = false, bool? links = false, int? page = null, int? sizePage = null);
    //    #endregion        

    //    #region Subscriptions
    //    IExecutableRequest<DetailedSubscription> CreateSubscription(string referenceCustomer, string referenceOffer, string referenceCustomerBuyer = null, bool? tryStart = null, DateTime? dateStart = null, 
    //        int? amountUpFront = null, int? amountTrial = null, TimeUnit? unitTrial = null, int? durationTrial = null, int? amountRecurrence = null, TimeUnit? unitRecurrence = null, int? durationRecurrence = null, 
    //        int? countRecurrences = null, int? countMinRecurrences = null, int? amountTermination = null, string titleLocalized = null, string descriptionLocalized = null, bool? html = null);
    //    IExecutableRequest<DetailedSubscription> RetrieveSubscription(long idSubscription, bool? html = null);
    //    IExecutableRequest<DetailedSubscription> RetrieveUniqueSubscription(string referenceCustomer, bool? html = null);
    //    IExecutableRequest<DetailedSubscription> SuspendSubscription(long idSubscription, SubscriptionState? suspensionState = null, bool? html = null);
    //    IExecutableRequest<DetailedSubscription> StartSubscription(long idSubscription, bool? html = null);
    //    IExecutableRequest<DetailedSubscription> TerminateSubscription(long idSubscription, bool immediate = false, DateTime? dateTermination = null, bool? html = null);
    //    IExecutableRequest<DetailedSubscription> UpgradeSubscription(long idSubscription, string referenceOffer = null, bool? html = null);
    //    IExecutableRequest<DetailedSubscription> SetSubscriptionRenewalDate(long idSubscription, DateTime dateRenewal, bool? html = null);
    //    IExecutableRequest<PaginatedList<Subscription>> ListSubscriptions(string referenceCustomer, string referenceCustomerBuyer, string referenceSegment, 
    //        bool? html = null, int? page = null, int? sizePage = null);
    //    #endregion
    //}
}