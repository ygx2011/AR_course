namespace UnityEngine.Purchasing.Models
{
    interface IGoogleBillingResult
    {
        GoogleBillingResponseCode responseCode {
            get;
        }
        string debugMessage {
            get;
        }
    }
}
