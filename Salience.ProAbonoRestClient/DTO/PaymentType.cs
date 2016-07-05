namespace ProAbono
{
    public enum PaymentType
    {
        /// <summary>
        /// Not defined yet
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// External payment : check
        /// </summary>
        ExternalCheck = 100,
        /// <summary>
        /// External payment : cash
        /// </summary>
        ExternalCash = 101,
        /// <summary>
        /// External payment : cash
        /// </summary>
        ExternalBank = 102,
        /// <summary>
        /// Undefined external payment
        /// </summary>
        ExternalOther = 120,

        /// <summary>
        /// Payment card
        /// </summary>
        Card = 1000,
        /// <summary>
        /// SEPA
        /// </summary>
        DirectDebit = 2000,
        /// <summary>
        /// Wallet
        /// </summary>
        Wallet = 3000,
    }
}