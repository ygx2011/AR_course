#if SERVICES_SDK_CORE_ENABLED

using Unity.Services.Core.Editor;

namespace UnityEditor.Purchasing
{
    internal class GameServicesAnalyticsPackageKeyHolder : IAnalyticsPackageKeyHolder
    {
        static IEditorGameServiceIdentifier s_Identifier;

        IEditorGameServiceIdentifier Identifier
        {
            get
            {
                if (s_Identifier == null)
                {
                    s_Identifier = EditorGameServiceRegistry.Instance.GetEditorGameService<PurchasingServiceIdentifier>().Identifier;
                }
                return s_Identifier;
            }
        }

        public string GetPackageKey()
        {
            return Identifier.GetKey();
        }
    }
}

#endif
