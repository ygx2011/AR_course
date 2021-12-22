using System;
using System.Collections.Generic;
using UnityEngine.Purchasing.Models;

namespace UnityEngine.Purchasing.Interfaces
{
    interface IGoogleQueryPurchasesService
    {
        void QueryPurchases(Action<List<GooglePurchase>> onQueryPurchaseSucceed);
    }
}
