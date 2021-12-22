using UnityEditor;

namespace UnityEngine.Advertisements.Editor
{
    static class AdsDashboardUrls
    {
        const string k_HomeFormat = "https://dashboard.unity3d.com/organizations/{0}/projects/{1}/monetization";

        public static string GetOverviewUrl()
            => FillUrlWithOrganizationAndProjectIds($"{k_HomeFormat}/overview");

        static string FillUrlWithOrganizationAndProjectIds(string url)
        {
#if ENABLE_EDITOR_GAME_SERVICES
            var organization = CloudProjectSettings.organizationKey;
#else
            var organization = CloudProjectSettings.organizationId;
#endif
            var filledUrl = string.Format(url, organization, CloudProjectSettings.projectId);
            return filledUrl;
        }
    }
}
