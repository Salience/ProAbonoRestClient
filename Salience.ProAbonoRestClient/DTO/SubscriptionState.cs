namespace ProAbono
{
    public static class SubscriptionState
    {
        /// <summary>
        /// The subscription has been initiated in the hosted pages.
        /// The customer has not chosen a payment mode yet.
        /// </summary>
        public const string InitiatedCustomer = "InitiatedCustomer";

        /// <summary>
        /// The subscription has been initiated in the agent pages (the backoffice).
        /// The customer has not chosen a payment mode yet.
        /// </summary>
        public const string InitiatedAgent = "InitiatedAgent";

        /// <summary>
        /// The subscription has been registered and will start with a delay.
        /// The user reached the payment step, but no payment occured yet.
        /// </summary>
        public const string Delayed = "Delayed";

        /// <summary>
        /// The subscription has been concluded and is currently active..
        /// </summary>
        public const string Running = "Running";

        /// <summary>
        /// The subscription has been suspended by its owner.
        /// </summary>
        public const string SuspendedCustomer = "SuspendedCustomer";

        /// <summary>
        /// The subscription has been suspended because the payment settings are missing.
        /// </summary>
        public const string SuspendedPaymentInfoMissing = "SuspendedPaymentInfoMissing";

        /// <summary>
        /// The subscription has been suspended because a payment is due.
        /// </summary>
        public const string SuspendedPaymentDue = "SuspendedPaymentDue";

        /// <summary>
        /// The subscription has been suspended by an administrator.
        /// </summary>
        public const string SuspendedAgent = "SuspendedAgent";

        /// <summary>
        /// The subscription has been suspended by the system.
        /// </summary>
        public const string SuspendedSystem = "SuspendedSystem";

        /// <summary>
        /// The subscription is over.
        /// </summary>
        public const string History = "History";

        /// <summary>
        /// The subscription has been terminated.
        /// </summary>
        public const string Terminated = "Terminated";

        /// <summary>
        /// The subscription will been terminated at the end of the current period.
        /// </summary>
        public const string TerminatedAtRenewal = "TerminatedAtRenewal";

        /// <summary>
        /// The subscription has been terminated by the customer.
        /// </summary>
        public const string TerminatedCustomer = "TerminatedCustomer";

        /// <summary>
        /// The subscription has been terminated by an administrator..
         /// </summary>
        public const string TerminatedAgent = "TerminatedAgent";

        /// <summary>
        /// The subscription has been terminated by the customer.
        /// </summary>
        public const string TerminatedRevokedCustomer = "TerminatedRevokedCustomer";

        /// <summary>
        /// The subscription has been terminated by an administrator.
        /// </summary>
        public const string TerminatedRevokedAgent = "TerminatedRevokedAgent";

        /// <summary>
        /// The subscription has been deleted
        /// </summary>
        public const string Deleted = "Deleted";
    }
}