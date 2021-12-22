using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace Samples.Purchasing.AppleAppStore.HandlingDeferredPurchases
{
    public class UserWarningAppleAppStore : MonoBehaviour
    {
        public Text warningText;

        public void UpdateWarningText()
        {
            var currentAppStore = StandardPurchasingModule.Instance().appStore;

            var warningMessage = currentAppStore != AppStore.AppleAppStore ?
                "This sample is meant to be tested using the Apple App Store.\n" +
                $"The currently selected store is: {currentAppStore}.\n" +
                "Build the project for iOS and use the Apple App Store.\n\n" +
                "See README for more information and instructions on how to test this sample."
                : "";

            warningText.text = warningMessage;
        }
    }
}
