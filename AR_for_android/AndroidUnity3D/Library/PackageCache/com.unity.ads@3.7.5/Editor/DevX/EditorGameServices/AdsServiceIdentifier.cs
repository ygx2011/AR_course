#if SERVICES_SDK_CORE_ENABLED
using Unity.Services.Core.Editor;

namespace UnityEngine.Advertisements.Editor
{
    /// <summary>
    /// Implementation of the <see cref="IEditorGameServiceIdentifier"/> for the Ads service.
    /// </summary>
    public struct AdsServiceIdentifier : IEditorGameServiceIdentifier
    {
        /// <inheritdoc/>
        public string GetKey() => "Ads";
    }
}
#endif
