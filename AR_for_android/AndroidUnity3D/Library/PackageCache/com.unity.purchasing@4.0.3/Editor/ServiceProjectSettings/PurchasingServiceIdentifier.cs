#if SERVICES_SDK_CORE_ENABLED

using Unity.Services.Core.Editor;

namespace UnityEditor.Purchasing
{
    internal struct PurchasingServiceIdentifier : IEditorGameServiceIdentifier
    {
        public string GetKey()
        {
            return PurchasingIdentifierKey.k_PurchasingKey;
        }
    }
}

#endif
