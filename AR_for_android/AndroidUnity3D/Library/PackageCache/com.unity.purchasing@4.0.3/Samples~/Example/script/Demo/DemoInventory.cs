using UnityEngine;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Mock game inventory mechanism that logs completion (a.k.a fulfillment) of a product transaction.
    /// </summary>
	[AddComponentMenu("")]
	public class DemoInventory : MonoBehaviour
	{
        /// <summary>
        /// Log <c>100.gold.coin</c> completion (aka fulfillment) of a product purchase transaction.
        /// </summary>
        /// <param name="productId"></param>
		public void Fulfill (string productId)
		{
			switch (productId) {
			case "100.gold.coins":
				Debug.Log ("You Got Money!");
				break;
			default:
				Debug.Log (
					string.Format (
						"Unrecognized productId \"{0}\"",
						productId
					)
				);
				break;
			}
		}
	}
}
