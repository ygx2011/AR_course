using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace Samples.Purchasing.GooglePlay.FraudDetection
{
    [RequireComponent(typeof(UserWarningGooglePlayStore))]
    public class FraudDetection : MonoBehaviour, IStoreListener
    {
        IStoreController m_StoreController;

        public User user;

        public string goldProductId = "com.mycompany.mygame.gold1";
        public ProductType goldType = ProductType.Consumable;

        public Text goldCountText;

        int m_GoldCount;

        void Start()
        {
            InitializePurchasing();
            UpdateWarningMessage();
        }

        void InitializePurchasing()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            var googlePlayConfiguration = builder.Configure<IGooglePlayConfiguration>();
            ConfigureGoogleFraudDetection(googlePlayConfiguration);

            builder.AddProduct(goldProductId, goldType);

            UnityPurchasing.Initialize(this, builder);
        }

        void ConfigureGoogleFraudDetection(IGooglePlayConfiguration googlePlayConfiguration)
        {
            //To make sure the account id and profile id do not contain personally identifiable information, we obfuscate this information by hashing it.
            var obfuscatedAccountId = HashString(user.AccountId);
            var obfuscatedProfileId = HashString(user.ProfileId);

            googlePlayConfiguration.SetObfuscatedAccountId(obfuscatedAccountId);
            googlePlayConfiguration.SetObfuscatedProfileId(obfuscatedProfileId);
        }

        static string HashString(string input)
        {
            var stringBuilder = new StringBuilder();
            foreach (var b in GetHash(input))
                stringBuilder.Append(b.ToString("X2"));

            return stringBuilder.ToString();
        }

        static IEnumerable<byte> GetHash(string input)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("In-App Purchasing successfully initialized");

            m_StoreController = controller;

            UpdateUI();
        }

        public void BuyGold()
        {
            m_StoreController.InitiatePurchase(goldProductId);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            var product = args.purchasedProduct;

            Debug.Log($"Processing Purchase: {product.definition.id}");

            UnlockContent(product);

            return PurchaseProcessingResult.Complete;
        }

        void UnlockContent(Product product)
        {
            if (product.definition.id == goldProductId)
            {
                AddGold();
            }

            UpdateUI();
        }

        void AddGold()
        {
            m_GoldCount++;
        }

        void UpdateUI()
        {
            goldCountText.text = $"Your Gold: {m_GoldCount}";
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log($"In-App Purchasing initialize failed: {error}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
        }

        void UpdateWarningMessage()
        {
            GetComponent<UserWarningGooglePlayStore>().UpdateWarningText();
        }
    }
}
