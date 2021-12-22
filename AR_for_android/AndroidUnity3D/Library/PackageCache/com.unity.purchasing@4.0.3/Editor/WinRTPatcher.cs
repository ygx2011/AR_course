using System;
using System.IO;
using UnityEngine;

namespace UnityEditor.Purchasing
{
    internal class WinRTPatcher
    {
        private const string k_OutputPath = "Assets/Scripts/UnityPurchasing/generated";

        private const string k_WorkaroundFileName = "WindowsRuntimeWorkaround.cs";
        private const string k_WorkaroundTemplateFileName = "WindowsRuntimeWorkaround.cs.template";

        internal static void PatchWinRTBuild()
        {
#if UNITY_2020
            if (!DoesWorkaroundClassExist())
            {
                try
                {
                    BuildWorkaroundClass();
                }
                catch (Exception patchException)
                {
                    Debug.LogWarning(patchException.StackTrace);
                }
            }

            AssetDatabase.Refresh();
#endif
        }

        private static bool DoesWorkaroundClassExist()
        {
            return File.Exists(GetFullPathForWorkaroundClass());
        }

        private static string GetFullPathForWorkaroundClass()
        {
            return Path.Combine(k_OutputPath, k_WorkaroundFileName);
        }

        private static void BuildWorkaroundClass()
        {
            string templateText = LoadTemplateText();

            if (templateText != null)
            {
                GeneratePatchFile(templateText);
            }
        }

        private static string LoadTemplateText()
        {
            string templateGUID = FindTemplateGUID(k_WorkaroundFileName);
            string templateText = null;

            if (templateGUID != null)
            {
                string templateAbsolutePath = Path.Combine(System.IO.Path.GetDirectoryName(Application.dataPath), AssetDatabase.GUIDToAssetPath(templateGUID));

                templateText = System.IO.File.ReadAllText(templateAbsolutePath);
            }
            else
            {
                Debug.LogError($"Could not find template \"{k_WorkaroundTemplateFileName}\".");
            }

            return templateText;
        }

        private static string FindTemplateGUID(string templateFilename)
        {
            string[] assetGUIDs = AssetDatabase.FindAssets(k_WorkaroundFileName);
            return (assetGUIDs.Length > 0) ? assetGUIDs[0] : null;
        }

        private static void GeneratePatchFile(string templateText)
        {
            Directory.CreateDirectory(k_OutputPath);
            File.WriteAllText(GetFullPathForWorkaroundClass(), templateText);
        }
    }
}
