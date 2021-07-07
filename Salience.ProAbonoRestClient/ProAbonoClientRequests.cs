using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using Salience.FluentApi;
using Salience.FluentApi.Internal;

namespace ProAbono
{
    public partial class ProAbonoClient : FluentClient
    {
        public ProAbonoClient(string businessId)
            : base(string.Format("https://api-{0}.proabono.com", businessId))
        {
            this.Serializer = new JsonSerializer
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                DateParseHandling = DateParseHandling.DateTime
            };
        }

        public void Authenticate(string agentKey, string apiKey)
        {
            this.SetAuthenticator(new HttpBasicAuthenticator(agentKey, apiKey));
        }

        protected override void HandleUnexpectedResponse(RequestData data)
        {
            var response = data.Response;
            if(response.ContentLength == 0)
                return;

            // deserialize error(s)
            Error[] errors;
            try
            {
                using(var reader = new StringReader(response.Content))
                {
                    var jsonReader = new JsonTextReader(reader);
                    if((int)response.StatusCode == 422)
                        errors = this.Serializer.Deserialize<Error[]>(jsonReader);
                    else
                        errors = new[] { this.Serializer.Deserialize<Error>(jsonReader) };
                }
            }
            catch(Exception)
            {
                errors = null;
            }

            if(errors != null && errors.Any())
                throw new ProAbonoException((int)response.StatusCode, errors);
        }

        #region Features
        /// <summary>
        /// Retrieve a feature
        /// </summary>
        /// <param name="referenceFeature">reference of requested feature</param>
        /// <param name="referenceSegment">the related segment (only used to localize the texts in the default language if 'language' hasn't been provided)</param>
        /// <param name="language">language of the localized texts</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text</param>
        /// <returns>The related feature</returns>
        private IExecutableRequest<Feature> RetrieveFeatureRequest(string referenceFeature, string referenceSegment = null, string language = null, bool? html = true)
        {
            Guard.NotNullOrEmpty(referenceFeature, "referenceFeature");

            return To("retrieve a feature")
                .Get("/v1/Feature", r => r
                    .AddParameter("ReferenceFeature", referenceFeature, ParameterType.QueryString)
                    .AddParameter("ReferenceSegment", referenceSegment, ParameterType.QueryString)
                    .AddParameter("Language", language, ParameterType.QueryString)
                    .AddParameter("Html", html, ParameterType.QueryString))
                .Expecting<Feature>();
        }

        /// <summary>
        /// Retrieve a feature
        /// </summary>
        /// <param name="referenceFeature">reference of requested feature</param>
        /// <param name="referenceCustomer">reference of the customer</param>
        /// <param name="language">language of the localized texts</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text</param>
        /// <returns>The related feature, with its current usage (aggregated among the user's subscriptions)</returns>
        private IExecutableRequest<Feature> RetrieveFeatureForCustomerRequest(string referenceFeature, string referenceCustomer, string language = null, bool? html = true)
        {
            Guard.NotNullOrEmpty(referenceFeature, "referenceFeature");
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");

            return To("retrieve a feature for a customer")
                .Get("/v1/Feature", r => r
                    .AddParameter("ReferenceFeature", referenceFeature, ParameterType.QueryString)
                    .AddParameter("ReferenceCustomer", referenceCustomer, ParameterType.QueryString)
                    .AddParameter("Language", language, ParameterType.QueryString)
                    .AddParameter("Html", html, ParameterType.QueryString))
                .Expecting<Feature>();
        }

        /// <summary>
        /// Retrieve a list of features
        /// </summary>
        /// <param name="referenceSegment">the related segment. If not provided and you have multiple segments, defaults on the first segment</param>
        /// <param name="isVisible">filter - get only visible features</param>
        /// <param name="language">language of the localized texts</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text</param>
        /// <param name="page">Pagination : page index (starts from 1) </param>
        /// <param name="sizePage">Pagination : page size (default is 10) </param>
        /// <returns>The related features</returns>
        private IExecutableRequest<PaginatedList<Feature>> RetrieveFeaturesRequest(string referenceSegment = null, bool? isVisible = null, string language = null, bool? html = true, int? page = null, int? sizePage = null)
        {
            return To("retrieve all features")
                .Get("/v1/Features", r => r
                    .AddParameter("ReferenceSegment", referenceSegment, ParameterType.QueryString)
                    .AddParameter("IsVisible", isVisible, ParameterType.QueryString)
                    .AddParameter("Language", language, ParameterType.QueryString)
                    .AddParameter("Html", html, ParameterType.QueryString)
                    .AddParameter("Page", page, ParameterType.QueryString)
                    .AddParameter("SizePage", sizePage, ParameterType.QueryString))
                .Expecting<PaginatedList<Feature>>()
                .Or(new PaginatedList<Feature>()).IfNoContent();
        }

        /// <summary>
        /// Retrieve the list of features available to a customer
        /// </summary>
        /// <param name="referenceCustomer">reference of the customer</param>
        /// <param name="isVisible">filter - get only visible features</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text</param>
        /// <param name="page">Pagination : page index (starts from 1) </param>
        /// <param name="sizePage">Pagination : page size (default is 10) </param>
        /// <returns>The related features, with the current usage of each feature (aggregated among the user's subscriptions)</returns>
        private IExecutableRequest<PaginatedList<Feature>> RetrieveFeaturesForCustomerRequest(string referenceCustomer, bool? isVisible = null, bool? html = true, int? page = null, int? sizePage = null)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");

            return To("retrieve features for a customer")
                .Get("/v1/Features", r => r
                    .AddParameter("ReferenceCustomer", referenceCustomer, ParameterType.QueryString)
                    .AddParameter("IsVisible", isVisible, ParameterType.QueryString)
                    .AddParameter("Html", html, ParameterType.QueryString)
                    .AddParameter("Page", page, ParameterType.QueryString)
                    .AddParameter("SizePage", sizePage, ParameterType.QueryString))
                .Expecting<PaginatedList<Feature>>()
                .Or(new PaginatedList<Feature>()).IfNoContent();
        }
        #endregion

        #region Customers
        /// <summary>
        /// Insert or update a customer
        /// </summary>
        /// <param name="referenceCustomer">new or existing customer reference</param>
        /// <param name="customerInfo">the customer properties</param>
        /// <param name="referenceSegment">the segment you want the push the customer into. If not provided and you have multiple segments, defaults on the first segment</param>
        /// <returns>The inserted/updated customer</returns>
        private IExecutableRequest<Customer> SaveCustomerRequest(string referenceCustomer, CustomerInfo customerInfo, string referenceSegment = null)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");
            Guard.NotNull(customerInfo, "customerInfo");

            return To("create a customer")
                .Post("/v1/Customer", r => r.AddJsonBody(new
                {
                    ReferenceSegment = referenceSegment,
                    ReferenceCustomer = referenceCustomer,
                    Name = customerInfo.Name,
                    Email = customerInfo.Email,
                    Language = customerInfo.Language
                }))
                .Expecting<Customer>();
        }

        /// <summary>
        /// Retrieve a customer
        /// </summary>
        /// <param name="referenceCustomer">reference of requested customer</param>
        /// <returns>the related customer</returns>
        private IExecutableRequest<Customer> RetrieveCustomerRequest(string referenceCustomer)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");

            return To("retrieve a customer")
                .Get("/v1/Customer", r => r.AddParameter("ReferenceCustomer", referenceCustomer, ParameterType.QueryString))
                .Expecting<Customer>();
        }

        /// <summary>
        /// Retrieve a list of customers
        /// </summary>
        /// <param name="referenceSegment">the related segment. If not provided and you have multiple segments, defaults on the first segment</param>
        /// <param name="page">Pagination : page index (starts from 1) </param>
        /// <param name="sizePage">Pagination : page size (default is 10) </param>
        /// <returns>the list of customers</returns>
        private IExecutableRequest<PaginatedList<Customer>> ListCustomersRequest(string referenceSegment = null, int? page = null, int? sizePage = null)
        {
            return To("list customers")
                .Get("/v1/Customers", r => r
                    .AddParameter("ReferenceSegment", referenceSegment, ParameterType.QueryString)
                    .AddParameter("Page", page, ParameterType.QueryString)
                    .AddParameter("SizePage", sizePage, ParameterType.QueryString))
                .Expecting<PaginatedList<Customer>>()
                .Or(new PaginatedList<Customer>()).IfNoContent();
        }

        /// <summary>
        /// Retrieve a list of customers having access to a given feature
        /// </summary>
        /// <param name="referenceFeature">specifies the feature the customers must have access to</param>
        /// <param name="referenceSegment">the related segment. If not provided and you have multiple segments, defaults on the first segment</param>
        /// <param name="page">Pagination : page index (starts from 1) </param>
        /// <param name="sizePage">Pagination : page size (default is 10) </param>
        /// <returns>the list of customers, with their current usage of the feature</returns>
        private IExecutableRequest<PaginatedList<CustomerWithUsage>> ListCustomersByFeatureRequest(string referenceFeature, string referenceSegment, int? page = null, int? sizePage = null)
        {
            Guard.NotNullOrEmpty(referenceFeature, "referenceFeature");

            return To("list customers by feature")
                .Get("/v1/Customers", r => r
                    .AddParameter("ReferenceFeature", referenceFeature, ParameterType.QueryString)
                    .AddParameter("ReferenceSegment", referenceSegment, ParameterType.QueryString)
                    .AddParameter("Page", page, ParameterType.QueryString)
                    .AddParameter("SizePage", sizePage, ParameterType.QueryString))
                .Expecting<PaginatedList<CustomerWithUsage>>()
                .Or(new PaginatedList<CustomerWithUsage>()).IfNoContent();
        }

        /// <summary>
        /// Retrieve the billing address of a customer
        /// </summary>
        /// <param name="referenceCustomer">reference of the customer</param>
        /// <returns>The address, or null if not found</returns>
        private IExecutableRequest<Address> RetrieveBillingAddressRequest(string referenceCustomer)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");

            return To("retrieve a billing address")
                .Get("/v1/CustomerBillingAddress", r => r.AddParameter("ReferenceCustomer", referenceCustomer, ParameterType.QueryString))
                .Expecting<Address>();
        }

        /// <summary>
        /// Insert or update the billing address of a customer
        /// </summary>
        /// <param name="referenceCustomer">reference of the customer</param>
        /// <param name="billingAddress">the new address</param>
        /// <returns>The inserted/updated address</returns>
        private IExecutableRequest<Address> SaveBillingAddressRequest(string referenceCustomer, Address billingAddress)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");
            Guard.NotNull(billingAddress, "billingAddress");

            return To("update a billing address")
                .Post("/v1/CustomerBillingAddress", r => r
                    .AddParameter("ReferenceCustomer", referenceCustomer, ParameterType.QueryString)
                    .AddJsonBody(billingAddress))
                .Expecting<Address>();
        }

        /// <summary>
        /// Retrieve the shipping address of a customer
        /// </summary>
        /// <param name="referenceCustomer">reference of the customer</param>
        /// <returns>The address, or null if not found</returns>
        private IExecutableRequest<Address> RetrieveShippingAddressRequest(string referenceCustomer)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");

            return To("retrieve a shipping address")
                .Get("/v1/CustomerShippingAddress", r => r.AddParameter("ReferenceCustomer", referenceCustomer, ParameterType.QueryString))
                .Expecting<Address>();
        }

        /// <summary>
        /// Insert or update the shipping address of a customer
        /// </summary>
        /// <param name="referenceCustomer">reference of the customer</param>
        /// <param name="shippingAddress">the new address</param>
        /// <returns>The inserted/updated address</returns>
        private IExecutableRequest<Address> SaveShippingAddressRequest(string referenceCustomer, Address shippingAddress)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");
            Guard.NotNull(shippingAddress, "shippingAddress");

            return To("update a shipping address")
                .Post("/v1/CustomerShippingAddress", r => r
                    .AddParameter("ReferenceCustomer", referenceCustomer, ParameterType.QueryString)
                    .AddJsonBody(shippingAddress))
                .Expecting<Address>();
        }

        /// <summary>
        /// Retrieve the payment settings of a customer
        /// </summary>
        /// <param name="referenceCustomer">reference of the customer</param>
        /// <returns>The settings, or null if not found</returns>
        private IExecutableRequest<PaymentSettings> RetrievePaymentSettingsRequest(string referenceCustomer)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");

            return To("retrieve payment settings")
                .Get("/v1/CustomerSettingsPayment", r => r.AddParameter("ReferenceCustomer", referenceCustomer, ParameterType.QueryString))
                .Expecting<PaymentSettings>();
        }

        /// <summary>
        /// Insert or update the payment settings of a customer
        /// </summary>
        /// <param name="referenceCustomer">reference of the customer</param>
        /// <param name="settings">the new settings</param>
        /// <returns>The inserted/updated settings</returns>
        private IExecutableRequest<PaymentSettings> SavePaymentSettingsRequest(string referenceCustomer, PaymentSettings settings)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");
            Guard.NotNull(settings, "settings");

            return To("update payment settings")
                .Post("/v1/CustomerSettingsPayment", r => r
                    .AddParameter("ReferenceCustomer", referenceCustomer, ParameterType.QueryString)
                    .AddJsonBody(settings))
                .Expecting<PaymentSettings>();
        }

        /// <summary>
        /// Anonymize a Customer
        /// </summary>
        /// <param name="referenceCustomer">reference of the customer</param>
        /// <returns>The updated customer</returns>
        private IExecutableRequest<Customer> AnonymizeCustomerRequest(string referenceCustomer)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");

            return To("anonymize a customer")
                .Post("/v1/Customer/Anonymization", r => r.AddParameter("ReferenceCustomer", referenceCustomer, ParameterType.QueryString))
                .Expecting<Customer>();
        }

        #endregion

        #region Usages
        /// <summary>
        /// Get a list of usages for a given customer
        /// </summary>
        /// <param name="referenceCustomer">reference of the customer</param>
        /// <param name="referenceFeature">filter - get only usages for that feature</param>
        /// <param name="aggregate">true to aggregate all usages of the same feature</param>
        /// <param name="page">Pagination : page index (starts from 1) </param>
        /// <param name="sizePage">Pagination : page size (default is 10) </param>
        /// <returns>The related usages</returns>
        private IExecutableRequest<PaginatedList<Usage>> RetrieveUsagesForCustomerRequest(string referenceCustomer, string referenceFeature = null, bool aggregate = false, int? page = null, int? sizePage = null)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");

            return To("retrieve usages for a customer")
                .Get("/v1/Usages", r => r
                    .AddParameter("ReferenceCustomer", referenceCustomer, ParameterType.QueryString)
                    .AddParameter("ReferenceFeature", referenceFeature, ParameterType.QueryString)
                    .AddParameter("Aggregate", aggregate, ParameterType.QueryString)
                    .AddParameter("Page", page, ParameterType.QueryString)
                    .AddParameter("SizePage", sizePage, ParameterType.QueryString))
                .Expecting<PaginatedList<Usage>>()
                .Or(new PaginatedList<Usage>()).IfNoContent();
        }

        /// <summary>
        /// Get a list of usages for a given subscription
        /// </summary>
        /// <param name="idSubscription">id of the subscription</param>
        /// <param name="page">Pagination : page index (starts from 1) </param>
        /// <param name="sizePage">Pagination : page size (default is 10) </param>
        /// <returns>The related usages</returns>
        private IExecutableRequest<PaginatedList<Usage>> RetrieveUsagesForSubscriptionRequest(long idSubscription, int? page = null, int? sizePage = null)
        {
            return To("retrieve usages for a customer")
                .Get("/v1/Usages", r => r
                    .AddParameter("IdSubscription", idSubscription, ParameterType.QueryString)
                    .AddParameter("Page", page, ParameterType.QueryString)
                    .AddParameter("SizePage", sizePage, ParameterType.QueryString))
                .Expecting<PaginatedList<Usage>>()
                .Or(new PaginatedList<Usage>()).IfNoContent();
        }

        /// <summary>
        /// Get a list of usages for a given feature
        /// </summary>
        /// <param name="referenceFeature">reference of the feature</param>
        /// <param name="aggregate">true to aggregate all usages of that feature for the same customer</param>
        /// <param name="page">Pagination : page index (starts from 1) </param>
        /// <param name="sizePage">Pagination : page size (default is 10) </param>
        /// <returns>The related usages</returns>
        private IExecutableRequest<PaginatedList<Usage>> RetrieveUsagesForFeatureRequest(string referenceFeature, bool aggregate = false, int? page = null, int? sizePage = null)
        {
            return To("retrieve usages for a feature")
                .Get("/v1/Usages", r => r
                    .AddParameter("ReferenceFeature", referenceFeature, ParameterType.QueryString)
                    .AddParameter("Aggregate", aggregate, ParameterType.QueryString)
                    .AddParameter("Page", page, ParameterType.QueryString)
                    .AddParameter("SizePage", sizePage, ParameterType.QueryString))
                .Expecting<PaginatedList<Usage>>()
                .Or(new PaginatedList<Usage>()).IfNoContent();
        }

        /// <summary>
        /// Get usage for a given customer and feature.
        /// If the given feature is present in multiple subscriptions for the customer, then the usages are aggregated.
        /// </summary>
        /// <param name="referenceCustomer">reference of related customer</param>
        /// <param name="referenceFeature">reference of related feature</param>
        /// <returns>The related usage</returns>
        private IExecutableRequest<Usage> RetrieveUsageForCustomerRequest(string referenceCustomer, string referenceFeature)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");
            Guard.NotNullOrEmpty(referenceFeature, "referenceFeature");

            return To("retrieve usage for a customer")
                .Get("/v1/Usage", r => r
                    .AddParameter("ReferenceFeature", referenceFeature, ParameterType.QueryString)
                    .AddParameter("ReferenceCustomer", referenceCustomer, ParameterType.QueryString))
                .Expecting<Usage>();
        }

        /// <summary>
        /// Get usage for a given subscription and feature
        /// </summary>
        /// <param name="idSubscription">reference of related subscription</param>
        /// <param name="referenceFeature">reference of related feature</param>
        /// <returns>The related usage</returns>
        private IExecutableRequest<Usage> RetrieveUsageForSubscriptionRequest(long idSubscription, string referenceFeature)
        {
            Guard.NotNullOrEmpty(referenceFeature, "referenceFeature");

            return To("retrieve usage for a subscription")
                .Get("/v1/Usage", r => r
                    .AddParameter("ReferenceFeature", referenceFeature, ParameterType.QueryString)
                    .AddParameter("IdSubscription", idSubscription, ParameterType.QueryString))
                .Expecting<Usage>();
        }

        /// <summary>
        /// Update usage for a given customer and feature
        /// </summary>
        /// <param name="referenceCustomer">reference of related customer</param>
        /// <param name="referenceFeature">reference of related feature</param>
        /// <param name="increment">the increment to apply to current usage</param>
        /// <param name="dateStamp">the date the usage update occurs, for concurrency issues. Defaults to now (UTC)</param>
        /// <param name="idSubscription">reference of related subscription</param>
        /// <returns>The related usage</returns>
        private IExecutableRequest<Usage> UpdateUsageByIncrementRequest(string referenceCustomer, string referenceFeature, int increment, DateTime? dateStamp = null, long? idSubscription = null)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");
            Guard.NotNullOrEmpty(referenceFeature, "referenceFeature");

            return To("update usage by increment")
                .Post("/v1/Usage", r => r
                    .AddJsonBody(new
                    {
                        ReferenceFeature = referenceFeature,
                        ReferenceCustomer = referenceCustomer,
                        Increment = increment,
                        DateStamp = dateStamp ?? DateTime.UtcNow,
                        IdSubscription = idSubscription
                    }))
                .Expecting<Usage>();
        }

        /// <summary>
        /// Update usage for a given customer and feature
        /// </summary>
        /// <param name="referenceCustomer">reference of related customer</param>
        /// <param name="referenceFeature">reference of related feature</param>
        /// <param name="quantityCurrent">the new usage quantity</param>
        /// <param name="dateStamp">the date the usage update occurs, for concurrency issues. Defaults to now (UTC)</param>
        /// <param name="idSubscription">reference of related subscription</param>
        /// <returns>The related usage</returns>
        private IExecutableRequest<Usage> UpdateUsageByCurrentQuantityRequest(string referenceCustomer, string referenceFeature, int quantityCurrent, DateTime? dateStamp = null, long? idSubscription = null)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");
            Guard.NotNullOrEmpty(referenceFeature, "referenceFeature");

            return To("update usage by current quantity")
                .Post("/v1/Usage", r => r
                    .AddJsonBody(new
                    {
                        ReferenceFeature = referenceFeature,
                        ReferenceCustomer = referenceCustomer,
                        QuantityCurrent = quantityCurrent,
                        DateStamp = dateStamp ?? DateTime.UtcNow,
                        IdSubscription = idSubscription
                    }))
                .Expecting<Usage>();
        }

        /// <summary>
        /// Update usage for a given customer and feature
        /// </summary>
        /// <param name="referenceCustomer">reference of related customer</param>
        /// <param name="referenceFeature">reference of related feature</param>
        /// <param name="isEnabled">toggle usage on an on/off feature</param>
        /// <param name="dateStamp">the date the usage update occurs, for concurrency issues. Defaults to now (UTC)</param>
        /// <param name="idSubscription">reference of related subscription</param>
        /// <returns>The related usage</returns>
        private IExecutableRequest<Usage> UpdateUsageByActivationRequest(string referenceCustomer, string referenceFeature, bool isEnabled, DateTime? dateStamp = null, long? idSubscription = null)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");
            Guard.NotNullOrEmpty(referenceFeature, "referenceFeature");

            return To("update usage of onoff feature")
                .Post("/v1/Usage", r => r
                    .AddJsonBody(new
                    {
                        ReferenceFeature = referenceFeature,
                        ReferenceCustomer = referenceCustomer,
                        IdSubscription = idSubscription,
                        IsEnabled = isEnabled,
                        DateStamp = dateStamp ?? DateTime.UtcNow,
                    }))
                .Expecting<Usage>();
        }
        #endregion

        #region Pricing
        /// <summary>
        /// Estimate the exact price for an usage update
        /// </summary>
        /// <param name="referenceCustomer">reference of related customer</param>
        /// <param name="referenceFeature">reference of the related feature</param>
        /// <param name="idSubscription">reference of related subscription</param>
        /// <param name="increment">the number of units to increment that usage</param>
        /// <param name="dateStamp">the date the usage update occurs, for concurrency issues. Defaults to now (UTC)</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text. Default is true</param>
        /// <returns>The corresponding pricing</returns>
        private IExecutableRequest<Pricing> EstimateUsageUpdateByIncrementRequest(string referenceCustomer, string referenceFeature, int increment, long? idSubscription = null, DateTime? dateStamp = null, bool? html = true)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");
            Guard.NotNullOrEmpty(referenceFeature, "referenceFeature");

            return To("estimate pricing of an usage update")
                .Post("/v1/PricingUsage", r => r
                    .AddJsonBody(new
                    {
                        ReferenceFeature = referenceFeature,
                        ReferenceCustomer = referenceCustomer,
                        Increment = increment,
                        DateStamp = dateStamp ?? DateTime.UtcNow,
                        IdSubscription = idSubscription,
                        Html = html
                    }))
                .Expecting<Pricing>();
        }

        /// <summary>
        /// Estimate the exact price for an usage update
        /// </summary>
        /// <param name="referenceCustomer">reference of related customer</param>
        /// <param name="referenceFeature">reference of the related feature</param>
        /// <param name="idSubscription">reference of related subscription</param>
        /// <param name="quantityCurrent">the new value of the usage (On-Off features only)</param>
        /// <param name="dateStamp">the date the usage update occurs, for concurrency issues. Defaults to now (UTC)</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text. Default is true</param>
        /// <returns>The corresponding pricing</returns>
        private IExecutableRequest<Pricing> EstimateUsageUpdateByCurrentQuantityRequest(string referenceCustomer, string referenceFeature, int quantityCurrent, long? idSubscription = null, DateTime? dateStamp = null, bool? html = true)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");
            Guard.NotNullOrEmpty(referenceFeature, "referenceFeature");

            return To("estimate pricing of an usage update")
                .Post("/v1/PricingUsage", r => r
                    .AddJsonBody(new
                    {
                        ReferenceFeature = referenceFeature,
                        ReferenceCustomer = referenceCustomer,
                        QuantityCurrent = quantityCurrent,
                        DateStamp = dateStamp ?? DateTime.UtcNow,
                        IdSubscription = idSubscription,
                        Html = html
                    }))
                .Expecting<Pricing>();
        }

        /// <summary>
        /// Estimate the exact price for an usage update
        /// </summary>
        /// <param name="referenceCustomer">reference of related customer</param>
        /// <param name="referenceFeature">reference of the related feature</param>
        /// <param name="idSubscription">reference of related subscription</param>
        /// <param name="isEnabled">the new status of given on-off usage</param>
        /// <param name="dateStamp">the date the usage update occurs, for concurrency issues. Defaults to now (UTC)</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text. Default is true</param>
        /// <returns>The corresponding pricing</returns>
        private IExecutableRequest<Pricing> EstimateUsageUpdateByActivationRequest(string referenceCustomer, string referenceFeature, bool isEnabled, long? idSubscription = null, DateTime? dateStamp = null, bool? html = true)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");
            Guard.NotNullOrEmpty(referenceFeature, "referenceFeature");

            return To("estimate pricing of an usage update")
                .Post("/v1/PricingUsage", r => r
                    .AddJsonBody(new
                    {
                        ReferenceFeature = referenceFeature,
                        ReferenceCustomer = referenceCustomer,
                        IsEnabled = isEnabled,
                        DateStamp = dateStamp ?? DateTime.UtcNow,
                        IdSubscription = idSubscription,
                        Html = html
                    }))
                .Expecting<Pricing>();
        }

        /// <summary>
        /// Compute the exact price for given customer to subscribe to given offer
        /// If there is a trial, the pricing will contain the pricing for the first billing period and the pricing for the next term
        /// You can override the offer's parameters in case you need a customized subscription
        /// </summary>
        /// <param name="referenceOffer">reference of the related offer, required if idOffer is null</param>
        /// <param name="referenceCustomer">reference of related customer</param>
        /// <param name="referenceCustomerBuyer">reference of related buyer, if the buyer is not the recipient of the subscription</param>
        /// <param name="tryStart">if true, will check if the customer is billable. if he's not then an error is returned</param>
        /// <param name="dateStart">specify the subscription's start date</param>
        /// <param name="amountUpFront">Offer override - upfront amount </param>
        /// <param name="amountTrial">Offer override - trial period amount</param>
        /// <param name="unitTrial">Offer override - trial period duration unit</param>
        /// <param name="durationTrial">Offer override - trial period duration</param>
        /// <param name="amountRecurrence">Offer override - recurrence amount</param>
        /// <param name="unitRecurrence">Offer override - recurrence period unit</param>
        /// <param name="durationRecurrence">Offer override - recurrence period duration</param>
        /// <param name="countRecurrences">Offer override - number of billing periods (1 or more, null for infinite)</param>
        /// <param name="countMinRecurrences">Offer override - minimum number of billing periods (engagement)</param>
        /// <param name="amountTermination">Offer override - termination fee</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text. Default is true</param>
        /// <returns>The corresponding pricing</returns>
        private IExecutableRequest<Pricing> EstimateSubscriptionUpgradeRequest(string referenceCustomer, string referenceOffer, string referenceCustomerBuyer = null, bool? tryStart = null, 
            DateTime? dateStart = null, int? amountUpFront = null, int? amountTrial = null, TimeUnit? unitTrial = null, int? durationTrial = null, int? amountRecurrence = null, 
            TimeUnit? unitRecurrence = null, int? durationRecurrence = null, int? countRecurrences = null, int? countMinRecurrences = null, int? amountTermination = null, bool? html = true)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");
            Guard.NotNullOrEmpty(referenceOffer, "referenceOffer");

            return To("estimate pricing of a subscription upgrade")
                .Post("/v1/PricingSubscription", r => r
                    .AddJsonBody(new
                    {
                        ReferenceCustomer = referenceCustomer,
                        ReferenceOffer = referenceOffer,
                        ReferenceCustomerBuyer = referenceCustomerBuyer,
                        TryStart = tryStart,
                        DateStart = dateStart,
                        AmountUpFront = amountUpFront,
                        AmountTrial = amountTrial,
                        UnitTrial = unitTrial,
                        DurationTrial = durationTrial,
                        AmountRecurrence = amountRecurrence,
                        UnitRecurrence = unitRecurrence,
                        DurationRecurrence = durationRecurrence,
                        CountRecurrences = countRecurrences,
                        CountMinRecurrences = countMinRecurrences,
                        AmountTermination = amountTermination,
                        Html = html
                    }))
                .Expecting<Pricing>();
        }
        #endregion

        #region Offers
        /// <summary>
        /// Retrieve an offer
        /// </summary>
        /// <param name="referenceOffer">reference of requested offer</param>
        /// <param name="referenceSegment">the related segment. If not provided and you have multiple segments, defaults on the first segment</param>
        /// <param name="language">language of the localized texts</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text</param>
        /// <param name="ignoreFeatures">true to prevent returning contained features (for faster call)</param>
        /// <param name="links">true to prevent returning related links (for faster call)</param>
        /// <returns>The related offer</returns>
        private IExecutableRequest<Offer> RetrieveOfferRequest(string referenceOffer, string referenceSegment = null, string language = null, bool? html = true, bool? ignoreFeatures = false, bool? links = false)
        {
            Guard.NotNullOrEmpty(referenceOffer, "referenceOffer");

            return To("retrieve an offer")
                .Get("/v1/Offer", r => r
                    .AddParameter("ReferenceSegment", referenceSegment, ParameterType.QueryString)
                    .AddParameter("ReferenceOffer", referenceOffer, ParameterType.QueryString)
                    .AddParameter("Language", language, ParameterType.QueryString)
                    .AddParameter("Html", html, ParameterType.QueryString)
                    .AddParameter("IgnoreFeatures", ignoreFeatures, ParameterType.QueryString)
                    .AddParameter("Links", links, ParameterType.QueryString))
                .Expecting<Offer>();
        }

        /// <summary>
        /// Retrieve an offer for a given customer
        /// </summary>
        /// <param name="referenceCustomer">reference of the customer</param>
        /// <param name="referenceOffer">reference of requested offer</param>
        /// <param name="ignoreFeatures">true to prevent returning contained features (for faster call)</param>
        /// <param name="isVisible">filter - ignored if ignoreFeatures has been specified. If true, returns only visible features</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text</param>
        /// <returns>The related offer, with customer-specific 'subscribe' links to allow the customer to subscribe to the offer</returns>
        private IExecutableRequest<Offer> RetrieveOfferForCustomerRequest(string referenceCustomer, string referenceOffer, bool? html = true, bool? ignoreFeatures = false, bool? isVisible = null)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");
            Guard.NotNullOrEmpty(referenceOffer, "referenceOffer");

            return To("retrieve an offer for a customer")
                .Get("/v1/Offer", r => r
                    .AddParameter("ReferenceCustomer", referenceCustomer, ParameterType.QueryString)
                    .AddParameter("ReferenceOffer", referenceOffer, ParameterType.QueryString)
                    .AddParameter("Html", html, ParameterType.QueryString)
                    .AddParameter("IgnoreFeatures", ignoreFeatures, ParameterType.QueryString)
                    .AddParameter("IsVisible", isVisible, ParameterType.QueryString))
                .Expecting<Offer>();
        }

        /// <summary>
        /// Retrieve a list of offers
        /// </summary>
        /// <param name="referenceSegment">the related segment. If not provided and you have multiple segments, defaults on the first segment</param>
        /// <param name="language">language of the localized texts</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text</param>
        /// <param name="ignoreFeatures">true to prevent returning contained features (for faster call)</param>
        /// <param name="isVisible">filter - If true, returns only visible offers (with their visible features if 'ignoreFeatures' is true). If false, returns only hidden offers</param>
        /// <param name="links">true to prevent returning related links (for faster call)</param>
        /// <param name="page">Pagination : page index (starts from 1) </param>
        /// <param name="sizePage">Pagination : page size (default is 10) </param>
        /// <returns>The related offers</returns>
        private IExecutableRequest<PaginatedList<Offer>> RetrieveOffersRequest(string referenceSegment = null, string language = null, bool? html = true, bool? ignoreFeatures = false, bool? isVisible = null, bool? links = false, int? page = null, int? sizePage = null)
        {
            return To("retrieve offers")
                .Get("/v1/Offers", r => r
                    .AddParameter("ReferenceSegment", referenceSegment, ParameterType.QueryString)
                    .AddParameter("Language", language, ParameterType.QueryString)
                    .AddParameter("Html", html, ParameterType.QueryString)
                    .AddParameter("IsVisible", isVisible, ParameterType.QueryString)
                    .AddParameter("IgnoreFeatures", ignoreFeatures, ParameterType.QueryString)
                    .AddParameter("Links", links, ParameterType.QueryString)
                    .AddParameter("Page", page, ParameterType.QueryString)
                    .AddParameter("SizePage", sizePage, ParameterType.QueryString))
                .Expecting<PaginatedList<Offer>>()
                .Or(new PaginatedList<Offer>()).IfNoContent();
        }

        /// <summary>
        /// Retrieve a list of offers for a given customer
        /// </summary>
        /// <param name="referenceCustomer">reference of the customer</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text</param>
        /// <param name="isVisible">filter - If true, returns only visible offers (with their visible features if 'ignoreFeatures' is true). If false, returns only hidden offers</param>
        /// <param name="ignoreFeatures">true to prevent returning contained features (for faster call)</param>
        /// <param name="page">Pagination : page index (starts from 1) </param>
        /// <param name="sizePage">Pagination : page size (default is 10) </param>
        /// <returns>The related offers, with customer-specific 'subscribe' links to allow the customer to subscribe to each offer</returns>
        private IExecutableRequest<PaginatedList<Offer>> RetrieveOffersForCustomerRequest(string referenceCustomer, bool? html = true, bool? isVisible = null, bool? ignoreFeatures = false, int? page = null, int? sizePage = null)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");

            return To("retrieve offers for a customer")
                .Get("/v1/Offers", r => r
                    .AddParameter("ReferenceCustomer", referenceCustomer, ParameterType.QueryString)
                    .AddParameter("Html", html, ParameterType.QueryString)
                    .AddParameter("IsVisible", isVisible, ParameterType.QueryString)
                    .AddParameter("IgnoreFeatures", ignoreFeatures, ParameterType.QueryString)
                    .AddParameter("Page", page, ParameterType.QueryString)
                    .AddParameter("SizePage", sizePage, ParameterType.QueryString))
                .Expecting<PaginatedList<Offer>>()
                .Or(new PaginatedList<Offer>()).IfNoContent();
        }

        /// <summary>
        /// Retrieve an offer to upgrade the subscription of a given customer
        /// </summary>
        /// <param name="referenceCustomer">reference of the customer</param>
        /// <param name="referenceOffer">reference of requested offer</param>
        /// <param name="idSubscription">the id of the subscription to upgrade</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text</param>
        /// <param name="ignoreFeatures">true to prevent returning contained features (for faster call)</param>
        /// <returns>The related offer, with customer-specific 'upgrade' links to allow the customer to subscribe to the offer</returns>
        private IExecutableRequest<Offer> RetrieveOfferToUpgradeCustomerRequest(string referenceCustomer, string referenceOffer, long? idSubscription = null, bool? html = true, bool? ignoreFeatures = false)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");
            Guard.NotNullOrEmpty(referenceOffer, "referenceOffer");

            return To("retrieve offers to upgrade a customer")
                .Get("/v1/Offer", r => r
                    .AddParameter("ReferenceCustomer", referenceCustomer, ParameterType.QueryString)
                    .AddParameter("ReferenceOffer", referenceOffer, ParameterType.QueryString)
                    .AddParameter("Upgrade", true, ParameterType.QueryString)
                    .AddParameter("IdSubscription", idSubscription, ParameterType.QueryString)
                    .AddParameter("Html", html, ParameterType.QueryString)
                    .AddParameter("IgnoreFeatures", ignoreFeatures, ParameterType.QueryString))
                .Expecting<Offer>();
        }

        /// <summary>
        /// Retrieve a list of offers to upgrade the subscription of a given customer
        /// </summary>
        /// <param name="referenceCustomer">reference of the customer</param>
        /// <param name="idSubscription">the id of the subscription to upgrade</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text</param>
        /// <param name="isVisible">filter - If true, returns only visible offers (with their visible features if 'ignoreFeatures' is true). If false, returns only hidden offers</param>
        /// <param name="ignoreFeatures">true to prevent returning contained features (for faster call)</param>
        /// <param name="page">Pagination : page index (starts from 1) </param>
        /// <param name="sizePage">Pagination : page size (default is 10) </param>
        /// <returns>The related offers, with customer-specific 'upgrade' links to allow the customer to subscribe to each offer</returns>
        private IExecutableRequest<PaginatedList<Offer>> RetrieveOffersToUpgradeCustomerRequest(string referenceCustomer, long? idSubscription = null, bool? html = true, bool? isVisible = null, bool? ignoreFeatures = false, int? page = null, int? sizePage = null)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");

            return To("retrieve offers to upgrade a customer")
                .Get("/v1/Offers", r => r
                    .AddParameter("ReferenceCustomer", referenceCustomer, ParameterType.QueryString)
                    .AddParameter("Upgrade", true, ParameterType.QueryString)
                    .AddParameter("IdSubscription", idSubscription, ParameterType.QueryString)
                    .AddParameter("Html", html, ParameterType.QueryString)
                    .AddParameter("IsVisible", isVisible, ParameterType.QueryString)
                    .AddParameter("IgnoreFeatures", ignoreFeatures, ParameterType.QueryString)
                    .AddParameter("Page", page, ParameterType.QueryString)
                    .AddParameter("SizePage", sizePage, ParameterType.QueryString))
                .Expecting<PaginatedList<Offer>>()
                .Or(new PaginatedList<Offer>()).IfNoContent();
        }
        #endregion

        #region Subscriptions
        /// <summary>
        /// Create (and start) a subscription
        /// </summary>
        /// <param name="referenceOffer">reference of the related offer, required if idOffer is null</param>
        /// <param name="referenceCustomer">(mandatory) reference of related customer</param>
        /// <param name="referenceCustomerBuyer">reference of related buyer, if the buyer is not the recipient of the subscription</param>
        /// <param name="tryStart">if true, will force the start of the new subscription. An error is returned if the offer isn't free and the customer isn't billable.</param>
        /// <param name="ensureBillable">if true, will check if the customer is billable. If he's not then an error is returned</param>
        /// <param name="dateStart">specify the subscription's start date</param>
        /// <param name="amountUpFront">Offer override - upfront amount </param>
        /// <param name="amountTrial">Offer override - trial period amount</param>
        /// <param name="unitTrial">Offer override - trial period duration unit</param>
        /// <param name="durationTrial">Offer override - trial period duration</param>
        /// <param name="amountRecurrence">Offer override - recurrence amount</param>
        /// <param name="unitRecurrence">Offer override - recurrence period unit</param>
        /// <param name="durationRecurrence">Offer override - recurrence period duration</param>
        /// <param name="countRecurrences">Offer override - number of billing periods (1 or more, null for infinite)</param>
        /// <param name="countMinRecurrences">Offer override - minimum number of billing periods (engagement)</param>
        /// <param name="amountTermination">Offer override - termination fee</param>
        /// <param name="titleLocalized">Offer override - localized title</param>
        /// <param name="descriptionLocalized">Offer override - localized description</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text. Default is true</param>
        /// <returns>The related subscription</returns>
        private IExecutableRequest<Subscription> CreateSubscriptionRequest(string referenceOffer, string referenceCustomer, string referenceCustomerBuyer = null, bool? tryStart = null, bool? ensureBillable = null, DateTime? dateStart = null, 
            int? amountUpFront = null, int? amountTrial = null, TimeUnit? unitTrial = null, int? durationTrial = null, int? amountRecurrence = null, TimeUnit? unitRecurrence = null, int? durationRecurrence = null, 
            int? countRecurrences = null, int? countMinRecurrences = null, int? amountTermination = null, string titleLocalized = null, string descriptionLocalized = null, bool? html = null)
        {
            Guard.NotNullOrEmpty(referenceCustomer, "referenceCustomer");
            Guard.NotNullOrEmpty(referenceOffer, "referenceOffer");

            return To("create a subscription")
                .Post("/v1/Subscription", r => r
                    .AddParameter("tryStart", tryStart, ParameterType.QueryString)
                    .AddParameter("ensureBillable", ensureBillable, ParameterType.QueryString)
                    .AddJsonBody(new
                {
                    ReferenceCustomer = referenceCustomer,
                    ReferenceOffer = referenceOffer,
                    DateStart = dateStart,
                    AmountUpFront = amountUpFront,
                    AmountTrial = amountTrial,
                    UnitTrial = unitTrial,
                    DurationTrial = durationTrial,
                    AmountRecurrence = amountRecurrence,
                    UnitRecurrence = unitRecurrence,
                    DurationRecurrence = durationRecurrence,
                    CountRecurrences = countRecurrences,
                    CountMinRecurrences = countMinRecurrences,
                    AmountTermination = amountTermination,
                    TitleLocalized = titleLocalized,
                    DescriptionLocalized = descriptionLocalized,
                    ReferenceCustomerBuyer = referenceCustomerBuyer,
                    Html = html
                }))
                .Expecting<Subscription>();
        }

        /// <summary>
        /// Retrieve a subscription
        /// </summary>
        /// <param name="idSubscription">id of the requested subscription</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text. Default is true</param>
        /// <returns>The related subscription</returns>
        private IExecutableRequest<Subscription> RetrieveSubscriptionRequest(long idSubscription, bool? html = null)
        {
            return To("retrieve a subscription")
                .Get("/v1/Subscription", r => r
                    .AddParameter("IdSubscription", idSubscription, ParameterType.QueryString)
                    .AddParameter("Html", html, ParameterType.QueryString))
                .Expecting<Subscription>()
                .Or(null).IfNotFound();
        }

        /// <summary>
        /// Retrieve a running subscription for a customer.
        /// 
        /// Throws an error if the customer has multiple running subscriptions.
        /// </summary>
        /// <param name="referenceCustomer">Customer whose subscription is requested. </param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text. Default is true</param>
        /// <returns>The related subscription</returns>
        private IExecutableRequest<Subscription> RetrieveSubscriptionForCustomerRequest(string referenceCustomer, bool? html = null)
        {
            return To("retrieve a subscription for a customer")
                .Get("/v1/Subscription", r => r
                    .AddParameter("referenceCustomer", referenceCustomer, ParameterType.QueryString)
                    .AddParameter("Html", html, ParameterType.QueryString))
                .Expecting<Subscription>();
        }

        /// <summary>
        /// Suspend a subscription
        /// </summary>
        /// <param name="idSubscription">id of the requested subscription</param>
        /// <param name="subscriptionState">The subscription suspension status. default: SuspendedAgent</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text. Default is true</param>
        /// <returns>The related subscription</returns>
        private IExecutableRequest<Subscription> SuspendSubscriptionRequest(long idSubscription, string subscriptionState = null, bool? html = null)
        {
            return To("suspend a subscription")
                .Post("/v1/Subscription/{IdSubscription}/Suspension", r => r
                    .AddParameter("IdSubscription", idSubscription, ParameterType.UrlSegment)
                    .AddParameter("StateSubscription", subscriptionState, ParameterType.QueryString)
                    .AddParameter("Html", html, ParameterType.QueryString))
                .Expecting<Subscription>();
        }

        /// <summary>
        /// Start or restart a subscription
        /// </summary>
        /// <param name="idSubscription">id of the requested subscription</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text. Default is true</param>
        /// <returns>The related subscription</returns>
        private IExecutableRequest<Subscription> StartSubscriptionRequest(long idSubscription, bool? html = null)
        {
            return To("start a subscription")
                .Post("/v1/Subscription/{IdSubscription}/Start", r => r
                    .AddParameter("IdSubscription", idSubscription, ParameterType.UrlSegment)
                    .AddParameter("Html", html, ParameterType.QueryString))
                .Expecting<Subscription>();
        }

        /// <summary>
        /// Terminate a subscription
        /// </summary>
        /// <param name="idSubscription">id of the requested subscription</param>
        /// <param name="immediate">true to terminate the subscription immediately, false to terminate at the end of the billign period.</param>
        /// <param name="dateTermination">ignored if immediate is true. Date of termination, if you need to specify a date that is not the end of the billing period.</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text. Default is true</param>
        /// <returns>The related subscription</returns>
        private IExecutableRequest<Subscription> TerminateSubscriptionRequest(long idSubscription, bool immediate = false, DateTime? dateTermination = null, bool? html = null)
        {
            return To("terminate a subscription")
                .Post("/v1/Subscription/{IdSubscription}/Termination", r => r
                    .AddParameter("IdSubscription", idSubscription, ParameterType.UrlSegment)
                    .AddParameter("Immediate", immediate, ParameterType.QueryString)
                    .AddParameter("DateTermination", dateTermination, ParameterType.QueryString)
                    .AddParameter("Html", html, ParameterType.QueryString))
                .Expecting<Subscription>();
        }

        /// <summary>
        /// Upgrade a subscription
        /// </summary>
        /// <param name="idSubscription">id of the requested subscription</param>
        /// <param name="referenceOffer">reference of the related offer</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text. Default is true</param>
        /// <returns>The related subscription</returns>
        private IExecutableRequest<Subscription> UpgradeSubscriptionRequest(long idSubscription, string referenceOffer, bool? html = null)
        {
            Guard.NotNullOrEmpty(referenceOffer, "referenceOffer");

            return To("upgrade a subscription")
                .Post("/v1/Subscription/{IdSubscription}/Upgrade", r => r
                    .AddParameter("IdSubscription", idSubscription, ParameterType.UrlSegment)
                    .AddParameter("ReferenceOffer", referenceOffer, ParameterType.QueryString)
                    .AddParameter("Html", html, ParameterType.QueryString))
                .Expecting<Subscription>();
        }

        /// <summary>
        /// Change the term date of a subscription
        /// </summary>
        /// <param name="idSubscription">id of the requested subscription</param>
        /// <param name="dateTerm">the updated term date. Must be in the future</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text. Default is true</param>
        /// <returns>The related subscription</returns>
        private IExecutableRequest<Subscription> UpdateSubscriptionRenewalDateRequest(long idSubscription, DateTime dateTerm, bool? html = null)
        {
            Guard.Future(dateTerm, "dateTerm");

            return To("change the renewal date of a subscription")
                .Post("/v1/Subscription/{IdSubscription}/DateTerm", r => r
                    .AddParameter("IdSubscription", idSubscription, ParameterType.UrlSegment)
                    .AddParameter("Html", html, ParameterType.QueryString)
                    .AddJsonBody(new { DateTerm = dateTerm }))
                .Expecting<Subscription>();
        }

        /// <summary>
        /// Retrieve a list of subscriptions
        /// </summary>
        /// <param name="referenceCustomer">Filter - returns only subscription where given customer is the recipient</param>
        /// <param name="referenceCustomerBuyer">Filter - returns only subscription where given customer is the buyer</param>
        /// <param name="referenceSegment">the related segment, if not provided and you have multiple segments, defaults on the first segment</param>
        /// <param name="html">true to have the localized text as HTML string, false for plain text. Default is true</param>
        /// <param name="page">Pagination : page index (starts from 1) </param>
        /// <param name="sizePage">Pagination : page size (default is 10) </param>
        /// <returns>The list of subscriptions</returns>
        private IExecutableRequest<PaginatedList<Subscription>> ListSubscriptionsRequest(string referenceCustomer = null, string referenceCustomerBuyer = null, string referenceSegment = null, 
            bool? html = null, int? page = null, int? sizePage = null)
        {
            return To("list subscriptions")
                .Get("/v1/Subscriptions", r => r
                    .AddParameter("ReferenceCustomer", referenceCustomer, ParameterType.QueryString)
                    .AddParameter("ReferenceCustomerBuyer", referenceCustomerBuyer, ParameterType.QueryString)
                    .AddParameter("ReferenceSegment", referenceSegment, ParameterType.QueryString)
                    .AddParameter("Html", html, ParameterType.QueryString)
                    .AddParameter("Page", page, ParameterType.QueryString)
                    .AddParameter("SizePage", sizePage, ParameterType.QueryString))
                .Expecting<PaginatedList<Subscription>>()
                .Or(new PaginatedList<Subscription>()).IfNoContent();
        }
        #endregion
    }
}
