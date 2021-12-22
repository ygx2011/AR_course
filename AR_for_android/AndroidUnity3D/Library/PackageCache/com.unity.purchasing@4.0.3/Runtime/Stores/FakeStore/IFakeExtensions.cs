using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing
{
	internal interface IFakeExtensions : IStoreExtension
	{
		string unavailableProductId { get; set; }
	}
}
