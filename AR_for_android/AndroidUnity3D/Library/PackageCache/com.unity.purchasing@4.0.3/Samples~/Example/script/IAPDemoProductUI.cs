using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

/// <summary>
/// Renders a <typeparamref name="Product"/> user interface, allowing purchasing, and display of
/// various hardcoded and retrieved pieces of data about this product.
/// </summary>
public class IAPDemoProductUI : MonoBehaviour
{
    /// <summary>
    /// Allows purchasing this product, when enabled for purchase.
    /// </summary>
    public Button purchaseButton;
    /// <summary>
    /// Dumps the contents of this product's receipt to the log.
    /// </summary>
    public Button receiptButton;
    /// <summary>
    /// The localized title of this product.
    /// </summary>
    public Text titleText;
    /// <summary>
    /// The localized description of this product.
    /// </summary>
    public Text descriptionText;
    /// <summary>
    /// The localized price, using the device's current country-specific app store.
    /// </summary>
    public Text priceText;
    /// <summary>
    /// Displays <c>Available</c> or <c>Unavailable</c> depending upon whether the backing app store
    /// has record of this product identifier.
    /// </summary>
    public Text statusText;

    private string m_ProductID;
    private Action<string> m_PurchaseCallback;
    private string m_Receipt;

    /// <summary>
    /// Attach a <typeparamref name="Product"/> to this user interface element.
    /// </summary>
    /// <param name="p">The product to be rendered.</param>
    /// <param name="purchaseCallback">Triggered upon purchase initiation.</param>
    public void SetProduct(Product p, Action<string> purchaseCallback)
    {
        titleText.text = p.metadata.localizedTitle;
        descriptionText.text = p.metadata.localizedDescription;
        priceText.text = p.metadata.localizedPriceString;

        receiptButton.interactable = p.hasReceipt;
        m_Receipt = p.receipt;

        m_ProductID = p.definition.id;
        m_PurchaseCallback = purchaseCallback;

        statusText.text = p.availableToPurchase ? "Available" : "Unavailable";
    }

    /// <summary>
    /// For testing <typeparamref name="PurchaseProcessingResult.Pending"/>, shows a time in seconds before
    /// the <c>IAPDemo</c> will complete the transaction for this product.
    /// </summary>
    /// <param name="secondsRemaining">Shows a time in seconds in the user interface.</param>
    /// <seealso cref="UnityEngine.Purchasing.PurchaseProcessingResult.Pending"/>
    /// <seealso cref="UnityEngine.Purchasing.IStoreController.ConfirmPendingPurchase"/>
    public void SetPendingTime(int secondsRemaining)
    {
        statusText.text = "Pending " + secondsRemaining.ToString();
    }

    /// <summary>
    /// Triggers the purchasing <typeparamref name="Action"/> for this identifier.
    /// </summary>
    public void PurchaseButtonClick()
    {
        if (m_PurchaseCallback != null && !string.IsNullOrEmpty(m_ProductID))
        {
            m_PurchaseCallback(m_ProductID);
        }
    }

    /// <summary>
    /// Logs this <typeparamref name="Product.receipt"/> to the <typeparamref name="Debug.Log"/>.
    /// </summary>
    public void ReceiptButtonClick()
    {
        if (!string.IsNullOrEmpty(m_Receipt))
            Debug.Log("Receipt for " + m_ProductID + ": " + m_Receipt);
    }
}
