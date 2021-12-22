namespace UnityEngine.Purchasing.Interfaces
{
    interface IGoogleLastKnownProductService
    {
        string GetLastKnownProductId();

        void SetLastKnownProductId(string lastKnownProductId);

        GooglePlayProrationMode? GetLastKnownProrationMode();

        void SetLastKnownProrationMode(GooglePlayProrationMode? lastKnownProrationMode);
    }
}
