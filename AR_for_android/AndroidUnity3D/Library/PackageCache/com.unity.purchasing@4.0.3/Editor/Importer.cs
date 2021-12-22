using UnityEditor.Purchasing;
using UnityEditor.Build;
using System;

namespace UnityEditor
{
    [InitializeOnLoad]
    internal class PurchasingImporter
    {
        static PurchasingImporter()
        {
            PurchasingSettings.ApplyEnableSettings(EditorUserBuildSettings.activeBuildTarget);
        }
    }
}
