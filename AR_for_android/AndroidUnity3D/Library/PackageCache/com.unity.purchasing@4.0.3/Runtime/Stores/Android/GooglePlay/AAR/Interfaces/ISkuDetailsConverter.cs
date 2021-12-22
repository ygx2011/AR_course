using System;
using System.Collections.Generic;
using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing.Interfaces
{
    interface ISkuDetailsConverter
    {
        List<ProductDescription> ConvertOnQuerySkuDetailsResponse(IEnumerable<AndroidJavaObject> skus);
    }
}
