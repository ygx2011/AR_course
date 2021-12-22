using System.Collections.Generic;
using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Initializes Unity IAP with the <typeparamref name="Product"/>s defined in the default IAP <see cref="ProductCatalog"/>.
    /// Automatically initializes at runtime load when enabled in the GUI. <seealso cref="ProductCatalog.enableCodelessAutoInitialization"/>
    /// Manages <typeparamref name="IAPButton"/>s and <typeparamref name="IAPListener"/>s.
    /// </summary>
    public class CodelessIAPStoreListener : IStoreListener
    {
        private static CodelessIAPStoreListener instance;
        private List<IAPButton> activeButtons = new List<IAPButton>();
        private List<IAPListener> activeListeners = new List<IAPListener> ();
        private static bool unityPurchasingInitialized;

        /// <summary>
        /// For advanced scripted IAP actions, use this session's <typeparamref name="IStoreController"/> after initialization.
        /// </summary>
        /// <see cref="StoreController"/>
        protected IStoreController controller;
        /// <summary>
        /// For advanced scripted store-specific IAP actions, use this session's <typeparamref name="IExtensionProvider"/> after initialization.
        /// </summary>
        /// <see cref="ExtensionProvider"/>
        protected IExtensionProvider extensions;

        ConfigurationBuilder m_Builder;

        /// <summary>
        /// For adding <typeparamref name="ProductDefinition"/> this default <typeparamref name="ProductCatalog"/> is
        /// loaded from the Project
        /// when I am instantiated.
        /// </summary>
        /// <see cref="Instance"/>
        protected ProductCatalog catalog;

        /// <summary>
        /// Allows outside sources to know whether the successful initialization has completed.
        /// </summary>
        public static bool initializationComplete;

        [RuntimeInitializeOnLoadMethod]
        static void InitializeCodelessPurchasingOnLoad() {
            ProductCatalog catalog = ProductCatalog.LoadDefaultCatalog();
            if (catalog.enableCodelessAutoInitialization && !catalog.IsEmpty() && instance == null)
            {
                CreateCodelessIAPStoreListenerInstance();
            }
        }

        private static void InitializePurchasing()
        {
            StandardPurchasingModule module = StandardPurchasingModule.Instance();
            module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

            ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);

            IAPConfigurationHelper.PopulateConfigurationBuilder(ref builder, instance.catalog);
            instance.m_Builder = builder;

            UnityPurchasing.Initialize(instance, builder);

            unityPurchasingInitialized = true;
        }

        /// <summary>
        /// For advanced scripted store-specific IAP actions, use this session's <typeparamref name="IStoreConfiguration"/>s.
        /// Note, these instances are only available after initialization through Codeless IAP, currently.
        /// </summary>
        /// <typeparam name="T">A subclass of <typeparamref name="IStoreConfiguration"/> such as <typeparamref name="IAppleConfiguration"/></typeparam>
        /// <returns></returns>
        public T GetStoreConfiguration<T>() where T : IStoreConfiguration
        {
            return m_Builder.Configure<T>();
        }

        /// <summary>
        /// For advanced scripted store-specific IAP actions, use this session's <typeparamref name="IStoreExtension"/>s after initialization.
        /// </summary>
        /// <typeparam name="T">A subclass of <typeparamref name="IStoreExtension"/> such as <typeparamref name="IAppleExtensions"/></typeparam>
        /// <returns></returns>
        public T GetStoreExtensions<T>() where T : IStoreExtension
        {
            return extensions.GetExtension<T>();
        }


        private CodelessIAPStoreListener()
        {
            catalog = ProductCatalog.LoadDefaultCatalog();
        }

        /// <summary>
        /// Singleton of me. Initialized on first access.
        /// Also initialized by RuntimeInitializeOnLoadMethod if <typeparamref name="ProductCatalog.enableCodelessAutoInitialization"/>
        /// is true.
        /// </summary>
        public static CodelessIAPStoreListener Instance
        {
            get
            {
                if (instance == null)
                {
                    CreateCodelessIAPStoreListenerInstance();
                }
                return instance;
            }
        }

        /// <summary>
        /// Creates the static instance of CodelessIAPStoreListener and initializes purchasing
        /// </summary>
        private static void CreateCodelessIAPStoreListenerInstance()
        {
            instance = new CodelessIAPStoreListener();
            if (!unityPurchasingInitialized)
            {
                InitializePurchasing();
            }
        }

        /// <summary>
        /// For advanced scripted IAP actions, use this session's <typeparamref name="IStoreController"/> after
        /// initialization.
        /// </summary>
        /// <see cref="StoreController"/>
        public IStoreController StoreController
        {
            get { return controller; }
        }

        /// <summary>
        /// Inspect my <typeparamref name="ProductCatalog"/> for a product identifier.
        /// </summary>
        /// <param name="productID">Product identifier to look for in <see cref="catalog"/>. Note this is not the
        /// store-specific identifier.</param>
        /// <returns>Whether this identifier exists in <see cref="catalog"/></returns>
        /// <see cref="catalog"/>
        public bool HasProductInCatalog(string productID)
        {
            foreach (var product in catalog.allProducts)
            {
                if (product.id == productID)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Access a <typeparamref name="Product"/> for this app.
        /// </summary>
        /// <param name="productID">A product identifier to find as a <typeparamref name="Product"/></param>
        /// <returns>A <typeparamref name="Product"/> corresponding to <paramref name="productID"/>, or <c>null</c> if
        /// the identifier is not available.</returns>
        public Product GetProduct(string productID)
        {
            if (controller != null && controller.products != null && !string.IsNullOrEmpty(productID))
            {
                return controller.products.WithID(productID);
            }
            Debug.LogError("CodelessIAPStoreListener attempted to get unknown product " + productID);
            return null;
        }

        /// <summary>
        /// Register an <typeparamref name="IAPButton"/> to send IAP initialization and purchasing events.
        /// Use to making IAP functionality visible to the user.
        /// </summary>
        /// <param name="button">The <typeparamref name="IAPButton"/></param>
        public void AddButton(IAPButton button)
        {
            activeButtons.Add(button);
        }

        /// <summary>
        /// Stop sending initialization and purchasing events to an <typeparamref name="IAPButton"/>. Use when disabling
        /// the button, e.g. when closing a scene containing that button and wanting to prevent the user from making any
        /// IAP events for its product.
        /// </summary>
        /// <param name="button"></param>
        public void RemoveButton(IAPButton button)
        {
            activeButtons.Remove(button);
        }

        /// <summary>
        /// Register an <typeparamref name="IAPListener"/> to send IAP purchasing events.
        /// </summary>
        /// <param name="listener">Listener to receive IAP purchasing events</param>
        public void AddListener(IAPListener listener)
        {
            activeListeners.Add (listener);
        }

        /// <summary>
        /// Unregister an <typeparamref name="IAPListener"/> from IAP purchasing events.
        /// </summary>
        /// <param name="listener">Listener to no longer receive IAP purchasing events</param>
        public void RemoveListener(IAPListener listener)
        {
            activeListeners.Remove (listener);
        }

        /// <summary>
        /// Purchase a product by its identifier.
        /// Sends purchase failure event with <typeparamref name="PurchaseFailureReason.PurchasingUnavailable"/>
        /// to all registered IAPButtons if not yet successfully initialized.
        /// </summary>
        /// <param name="productID">Product identifier of <typeparamref name="Product"/> to be purchased</param>
        public void InitiatePurchase(string productID)
        {
            if (controller == null)
            {
                Debug.LogError("Purchase failed because Purchasing was not initialized correctly");

                foreach (var button in activeButtons)
                {
                    if (button.productId == productID)
                    {
                        button.OnPurchaseFailed(null, Purchasing.PurchaseFailureReason.PurchasingUnavailable);
                    }
                }
                return;
            }

            controller.InitiatePurchase(productID);
        }

        /// <summary>
        /// Implementation of <typeparamref name="UnityEngine.Purchasing.IStoreListener.OnInitialized"/> which captures
        /// successful IAP initialization results and refreshes all registered <typeparamref name="IAPButton"/>s.
        /// </summary>
        /// <param name="controller">Set as the current IAP session's single <typeparamref name="IStoreController"/></param>
        /// <param name="extensions">Set as the current IAP session's single <typeparamref name="IExtensionProvider"/></param>
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            initializationComplete = true;
            this.controller = controller;
            this.extensions = extensions;

            foreach (var button in activeButtons)
            {
                button.UpdateText();
            }
        }

        /// <summary>
        /// Implementation of <typeparamref name="UnityEngine.Purchasing.IStoreListener.OnInitializeFailed"/> which
        /// logs the failure reason.
        /// </summary>
        /// <param name="error">Reported in the app log</param>
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError(string.Format("Purchasing failed to initialize. Reason: {0}", error.ToString()));
        }

        /// <summary>
        /// Implementation of <typeparamref name="UnityEngine.Purchasing.IStoreListener.ProcessPurchase"/> which forwards
        /// this successful purchase event to any appropriate registered <typeparamref name="IAPButton"/>s and
        /// <typeparamref name="IAPListener"/>s. Logs an error if there are no appropriate registered handlers.
        /// </summary>
        /// <param name="e">Data for this purchase</param>
        /// <returns>Any indication of whether this purchase has been completed by any of my appropriate registered
        /// <typeparamref name="IAPButton"/>s or <typeparamref name="IAPListener"/>s</returns>
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            PurchaseProcessingResult result;

            // if any receiver consumed this purchase we return the status
            bool consumePurchase = false;
            bool resultProcessed = false;

            foreach (IAPButton button in activeButtons)
            {
                if (button.productId == e.purchasedProduct.definition.id)
                {
                    result = button.ProcessPurchase(e);

                    if (result == PurchaseProcessingResult.Complete) {

                        consumePurchase = true;
                    }

                    resultProcessed = true;
                }
            }

            foreach (IAPListener listener in activeListeners)
            {
                result = listener.ProcessPurchase(e);

                if (result == PurchaseProcessingResult.Complete) {

                    consumePurchase = true;
                }

                resultProcessed = true;
            }

            // we expect at least one receiver to get this message
            if (!resultProcessed) {

                Debug.LogError("Purchase not correctly processed for product \"" +
                                 e.purchasedProduct.definition.id +
                                 "\". Add an active IAPButton to process this purchase, or add an IAPListener to receive any unhandled purchase events.");

            }

            return (consumePurchase) ? PurchaseProcessingResult.Complete : PurchaseProcessingResult.Pending;
        }

        /// <summary>
        /// Implementation of <typeparamref name="UnityEngine.Purchasing.IStoreListener.OnPurchaseFailed"/> indicating
        /// a purchase failed with specified reason. Send this event to any appropriate registered
        /// <typeparamref name="IAPButton"/>s and <typeparamref name="IAPListener"/>s.
        /// Logs an error if there are no appropriate registered handlers.
        /// </summary>
        /// <param name="product">What failed to purchase</param>
        /// <param name="reason">Why the purchase failed</param>
        public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
        {
            bool resultProcessed = false;

            foreach (IAPButton button in activeButtons)
            {
                if (button.productId == product.definition.id)
                {
                    button.OnPurchaseFailed(product, reason);

                    resultProcessed = true;
                }
            }

            foreach (IAPListener listener in activeListeners)
            {
                listener.OnPurchaseFailed(product, reason);

                resultProcessed = true;
            }

            // we expect at least one receiver to get this message
            if (!resultProcessed)
            {

                Debug.LogError("Failed purchase not correctly handled for product \"" + product.definition.id +
                                  "\". Add an active IAPButton to handle this failure, or add an IAPListener to receive any unhandled purchase failures.");
            }
        }
    }
}
