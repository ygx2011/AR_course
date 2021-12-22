using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// A GUI component for exposing the current price and allow purchasing of In-App Purchases. Exposes configurable
    /// elements through the Inspector.
    /// </summary>
    /// <seealso cref="CodelessIAPStoreListener"/>
    [RequireComponent(typeof(Button))]
#if ENABLE_EDITOR_GAME_SERVICES
    [AddComponentMenu("In-App Purchasing/IAP Button")]
#else
    [AddComponentMenu("Unity IAP/IAP Button")]
#endif
    [HelpURL("https://docs.unity3d.com/Manual/UnityIAP.html")]
    public class IAPButton : MonoBehaviour
    {
        /// <summary>
        /// The type of this button, can be either a purchase or a restore button.
        /// </summary>
        public enum ButtonType
        {
            /// <summary>
            /// This button will display localized product title and price. Clicking will trigger a purchase.
            /// </summary>
            Purchase,
            /// <summary>
            /// This button will display a static string for restoring previously purchased non-consumable
            /// and subscriptions. Clicking will trigger this restoration process, on supported app stores.
            /// </summary>
            Restore
        }

        /// <summary>
        /// Type of event fired after a successful purchase of a product.
        /// </summary>
        [System.Serializable]
        public class OnPurchaseCompletedEvent : UnityEvent<Product>
        {
        };

        /// <summary>
        /// Type of event fired after a failed purchase of a product.
        /// </summary>
        [System.Serializable]
        public class OnPurchaseFailedEvent : UnityEvent<Product, PurchaseFailureReason>
        {
        };

        /// <summary>
        /// Which product identifier to represent. Note this is not a store-specific identifier.
        /// </summary>
        [HideInInspector]
        public string productId;

        /// <summary>
        /// The type of this button, can be either a purchase or a restore button.
        /// </summary>
        [Tooltip("The type of this button, can be either a purchase or a restore button.")]
        public ButtonType buttonType = ButtonType.Purchase;

        /// <summary>
        /// Consume the product immediately after a successful purchase.
        /// </summary>
        [Tooltip("Consume the product immediately after a successful purchase.")]
        public bool consumePurchase = true;

        /// <summary>
        /// Event fired after a successful purchase of this product.
        /// </summary>
        [Tooltip("Event fired after a successful purchase of this product.")]
        public OnPurchaseCompletedEvent onPurchaseComplete;

        /// <summary>
        /// Event fired after a failed purchase of this product.
        /// </summary>
        [Tooltip("Event fired after a failed purchase of this product.")]
        public OnPurchaseFailedEvent onPurchaseFailed;

        /// <summary>
        /// Displays the localized title from the app store.
        /// </summary>
        [Tooltip("[Optional] Displays the localized title from the app store.")]
        public Text titleText;

        /// <summary>
        /// Displays the localized description from the app store.
        /// </summary>
        [Tooltip("[Optional] Displays the localized description from the app store.")]
        public Text descriptionText;

        /// <summary>
        /// Displays the localized price from the app store.
        /// </summary>
        [Tooltip("[Optional] Displays the localized price from the app store.")]
        public Text priceText;

        void Start()
        {
            Button button = GetComponent<Button>();

            if (buttonType == ButtonType.Purchase)
            {
                if (button)
                {
                    button.onClick.AddListener(PurchaseProduct);
                }

                if (string.IsNullOrEmpty(productId))
                {
                    Debug.LogError("IAPButton productId is empty");
                }

                if (!CodelessIAPStoreListener.Instance.HasProductInCatalog(productId))
                {
                    Debug.LogWarning("The product catalog has no product with the ID \"" + productId + "\"");
                }
            }
            else if (buttonType == ButtonType.Restore)
            {
                if (button)
                {
                    button.onClick.AddListener(Restore);
                }
            }
        }

        void OnEnable()
        {
            if (buttonType == ButtonType.Purchase)
            {
                CodelessIAPStoreListener.Instance.AddButton(this);
                if (CodelessIAPStoreListener.initializationComplete) {
                    UpdateText();
                }
            }
        }

        void OnDisable()
        {
            if (buttonType == ButtonType.Purchase)
            {
                CodelessIAPStoreListener.Instance.RemoveButton(this);
            }
        }

        void PurchaseProduct()
        {
            if (buttonType == ButtonType.Purchase)
            {
                CodelessIAPStoreListener.Instance.InitiatePurchase(productId);
            }
        }

        void Restore()
        {
            if (buttonType == ButtonType.Restore)
            {
                if (Application.platform == RuntimePlatform.WSAPlayerX86 ||
                    Application.platform == RuntimePlatform.WSAPlayerX64 ||
                    Application.platform == RuntimePlatform.WSAPlayerARM)
                {
                    CodelessIAPStoreListener.Instance.GetStoreExtensions<IMicrosoftExtensions>()
                        .RestoreTransactions();
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer ||
                         Application.platform == RuntimePlatform.OSXPlayer ||
                         Application.platform == RuntimePlatform.tvOS)
                {
                    CodelessIAPStoreListener.Instance.GetStoreExtensions<IAppleExtensions>()
                        .RestoreTransactions(OnTransactionsRestored);
                }
                else if (Application.platform == RuntimePlatform.Android &&
                    StandardPurchasingModule.Instance().appStore == AppStore.GooglePlay)
                {
                    CodelessIAPStoreListener.Instance.GetStoreExtensions<IGooglePlayStoreExtensions>()
                        .RestoreTransactions(OnTransactionsRestored);
                }
                else
                {
                    Debug.LogWarning(Application.platform.ToString() +
                                     " is not a supported platform for the Codeless IAP restore button");
                }
            }
        }

        void OnTransactionsRestored(bool success)
        {
            //TODO: Add an invocation hook here for developers.
        }


        /// <summary>
        /// Invoke to process a successful purchase of the product associated with this button.
        /// </summary>
        /// <param name="e">The successful <c>PurchaseEventArgs</c> for the purchase event. </param>
        /// <returns>The result of the successful purchase</returns>
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            onPurchaseComplete.Invoke(e.purchasedProduct);

            return (consumePurchase) ? PurchaseProcessingResult.Complete : PurchaseProcessingResult.Pending;
        }

        /// <summary>
        /// Invoked on a failed purchase of the product associated with this button
        /// </summary>
        /// <param name="product">The <typeparamref name="Product"/> which failed to purchase</param>
        /// <param name="reason">Information to help developers recover from this failure</param>
        public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
        {
            onPurchaseFailed.Invoke(product, reason);
        }

        internal void UpdateText()
        {
            var product = CodelessIAPStoreListener.Instance.GetProduct(productId);
            if (product != null)
            {
                if (titleText != null)
                {
                    titleText.text = product.metadata.localizedTitle;
                }

                if (descriptionText != null)
                {
                    descriptionText.text = product.metadata.localizedDescription;
                }

                if (priceText != null)
                {
                    priceText.text = product.metadata.localizedPriceString;
                }
            }
        }
    }
}
