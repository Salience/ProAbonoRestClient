namespace ProAbono
{
    public enum SubscriptionState
    {
        /// <summary>
        /// Subscription has been initiated in the hosted pages
        /// The customer did not choose a payment mode
        /// </summary>
        InitiatedCustomer,

        /// <summary>
        /// Subscription has been initiated in the agent pages (the backoffice)
        /// The customer did not choose a payment mode
        /// </summary>
        InitiatedAgent,

        /// <summary>
        /// Subscription has been registered and will start with a delay
        /// The user reached the payment step, but no payment occured
        /// </summary>
        Delayed,

        /// <summary>
        /// Subscription has been concluded
        /// It is currently active
        /// </summary>
        Running,

        /// <summary>
        /// Subscription has been suspended by it's owner
        /// </summary>
        SuspendedCustomer,

        /// <summary>
        /// Subscription has been suspended because payment settings are missing
        /// </summary>
        SuspendedPaymentInfoMissing,

        /// <summary>
        /// Subscription has been suspended because a payment is due
        /// </summary>
        SuspendedPaymentDue,

        /// <summary>
        /// Subscription has been suspended by a business administrator
        /// </summary>
        SuspendedAgent,

        /// <summary>
        /// Subscription has been suspended by the system
        /// </summary>
        SuspendedSystem,

        /// <summary>
        /// Subscription has been concluded
        /// Subscription is over now, but the user can still access the features while the term is not reached
        /// </summary>
        History,

        /// <summary>
        /// Subscription has been terminated
        /// The subscription is over, but the user can still access the features while the term is not reached
        /// no update can be done anymore
        /// </summary>
        Terminated,
        /// <summary>
        /// Subscription has been terminated
        /// The subscription is over, but the user can still access the features while the term is not reached
        /// no update can be done anymore
        /// </summary>
        TerminatedCustomer,
        /// <summary>
        /// Subscription has been terminated by an administrator
        /// The subscription is over, but the user can still access the features while the term is not reached
        /// no update can be done anymore
        /// </summary>
        TerminatedAgent,

        /// <summary>
        /// Subscription has been terminated
        /// The subscription is over, the user cannot access the features anymore
        /// no update can be done anymore
        /// </summary>
        TerminatedRevokedCustomer,
        /// <summary>
        /// Subscription has been terminated by an administrator
        /// The subscription is over, the user cannot access the features anymore
        /// no update can be done anymore
        /// </summary>
        TerminatedRevokedAgent,

        /// <summary>
        /// Subscription has been deleted
        /// </summary>
        Deleted,
    }
}