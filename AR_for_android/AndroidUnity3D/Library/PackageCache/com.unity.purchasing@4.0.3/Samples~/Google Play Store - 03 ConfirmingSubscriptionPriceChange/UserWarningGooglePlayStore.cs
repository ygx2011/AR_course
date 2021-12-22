using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace Samples.Purchasing.GooglePlay.ConfirmingSubscriptionPriceChange
{
    public class UserWarningGooglePlayStore : MonoBehaviour
    {
        public Text warningText;

        public void UpdateWarningText()
        {
            var currentAppStore = StandardPurchasingModule.Instance().appStore;

            var warningMessage = currentAppStore != AppStore.GooglePlay ?
                "This sample is meant to be tested using the Google Play Store.\n" +
                $"The currently selected store is: {currentAppStore}.\n" +
                "Build the project for Android and use the Google Play Store.\n\n" +
                "See README for more information and instructions on how to test this sample."
                : "";

            warningText.text = warningMessage;
        }
    }
}
