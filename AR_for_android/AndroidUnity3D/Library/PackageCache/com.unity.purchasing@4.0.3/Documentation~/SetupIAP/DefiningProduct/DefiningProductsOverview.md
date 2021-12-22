# Defining products

## Product ID
Enter a cross-platform unique identifier to serve as the Product’s default ID when communicating with an app store. 

**Important**: The ID may only contain lowercase letters, numbers, underscores, or periods.

## Product Type
Each Product must be of one of the following Types:

| **Type** | **Description** | **Examples** |
|:---|:---|:---|
|__Consumable__| Users can purchase the Product repeatedly. Consumable Products cannot be restored. | * Virtual currencies <br/> * Health potions <br/> * Temporary power-ups. |
|__Non-Consumable__| Users can only purchase the Product once. Non-Consumable Products can be restored. | * Weapons or armor <br/> * Access to extra content <br/> * No ads  |
|__Subscription__| Users can access the Product for a finite period of time. Subscription Products can be restored. | * Monthly access to an online game <br/> * VIP status granting daily bonuses <br/> * A free trial |

## Product Metadata
This section defines the metadata associated with your Product for use in an in-game store.

### Descriptions
Use the following fields to add descriptive text for your Product:

| **Field** | **Data type** | **Description** | **Example** |
|:---|:---|:---|:---|
| __Product Locale__ | Enum | Determines the app stores available in your region. | **English (U.S.)** (Google Play, Apple) |
| __Product Title__ | String | The name of your Product as it appears in an app store. | “Health Potion” |
| __Product Description__ | String | The descriptive text for your Product as it appears in an app store, usually an explanation of what the Product is. | “Restores 50 HP.” |

### Payouts
Use this section to add local, fixed definitions for the content you pay out to the purchaser. Payouts make it easier to manage in-game wallets or inventories. By labeling a Product with a name and quantity, developers can quickly adjust in-game counts of certain item types upon purchase (for example, coins or gems).

| **Field** | **Data type** | **Description** | **Example** |
|:---|:---|:---|:---|
| __Payout Type__ | Enum | Defines the category of content the purchaser receives. There are four possible Types. | * Currency <br/> * Item<br/> * Resource <br/> * Other|
| __Payout Subtype__ | String | Provides a level of granularity to the content category. |* “Gold” and “Silver” subtypes of a __Currency__ type <br/> * “Potion” and “Boost” subtypes of an __Item__ type |
| __Quantity__ | Int | Specifies the number of items, currency, and so on, that the purchaser receives in the payout. | * 1 <br/> * &gt;25<br/>* 100|
| __Data__ | | Use this field any way you like as a property to reference in code. | * Flag for a UI element<br/> * Item rarity |  

**Note**: You can add multiple Payouts to a single Product. 

For more information on the PayoutDefinition class, see the [Scripting Reference](xref:UnityEngine.Purchasing.PayoutDefinition). You can always add Payout information to a Product in a script using this class. For example:

### Store ID Overrides
By default, Unity IAP assumes that your Product has the same identifier (specified in the **ID** field, above) across all app stores. Unity recommends doing this where possible. However, there are occasions when this is not possible, such as when publishing to both iOS and Mac stores, which prohibit developers from using the same product ID across both.

