using System;

namespace UnityEngine.Advertisements.Utilities {
    public static class EnumUtilities {
        public static ShowResult GetShowResultsFromCompletionState(UnityAdsShowCompletionState showCompletionState) {
            switch (showCompletionState) {
                case UnityAdsShowCompletionState.COMPLETED:
                    return ShowResult.Finished;
                case UnityAdsShowCompletionState.SKIPPED:
                    return ShowResult.Skipped;
                default:
                    return ShowResult.Failed;
            }
        }

        public static T GetEnumFromAndroidJavaObject<T>(AndroidJavaObject androidJavaObject, T defaultValue) {
            try {
                return (T) Enum.Parse(typeof(T), androidJavaObject.Call<string>("toString"), true);
            } catch (Exception) {
                Debug.LogError("Unable to map native enum to managed enum");
            }

            return defaultValue;
        }
    }
}
