using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor.Purchasing
{
    internal class ObfuscationGenerator
    {
        const string m_GeneratedCredentialsTemplateFilename = "IAPGeneratedCredentials.cs.template";
        const string m_GeneratedCredentialsTemplateFilenameNoExtension = "IAPGeneratedCredentials.cs";

        const string k_AppleCertPath = "Packages/com.unity.purchasing/Editor/AppleIncRootCertificate.cer";

        internal static string ObfuscateAppleSecrets()
        {
            var appleError = BuildObfuscatedAppleClass();

            AssetDatabase.Refresh();

            return appleError;

        }

        internal static string ObfuscateGoogleSecrets(string googlePlayPublicKey)
        {
            string googleError = BuildObfuscatedGooglePlayClass(googlePlayPublicKey);

            AssetDatabase.Refresh();

            return googleError;

        }

        /// <summary>
        /// Generates specified obfuscated class files.
        /// </summary>
        internal static void ObfuscateSecrets(bool includeGoogle, ref string appleError, ref string googleError, string googlePlayPublicKey)
        {
            try
            {
                // First things first! Obfuscate! XHTLOA!
                appleError = BuildObfuscatedAppleClass();

                if (includeGoogle)
                {
                    googleError = BuildObfuscatedGooglePlayClass(googlePlayPublicKey);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.StackTrace);
            }

            // Ensure all the Tangle classes exist, even if they were not generated at this time. Apple will always
            // be generated.
            if (!DoesGooglePlayTangleClassExist())
            {
                try
                {
                    BuildObfuscatedClass(TangleFileConsts.k_GooglePlayClassPrefix, 0, new int[0], new byte[0], false);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.StackTrace);
                }
            }

            AssetDatabase.Refresh();
        }

        static string BuildObfuscatedAppleClass()
        {
            string appleError = null;
            int key = 0;
            int[] order = new int[0];
            byte[] tangled = new byte[0];
            try
            {
                byte[] bytes = System.IO.File.ReadAllBytes(k_AppleCertPath);
                order = new int[bytes.Length / 20 + 1];

                // TODO: Integrate with upgraded Tangle!

                tangled = TangleObfuscator.Obfuscate(bytes, order, out key);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Invalid Apple Root Certificate. Generating incomplete credentials file. " + e);
                appleError = "  Invalid Apple Root Certificate";
            }
            BuildObfuscatedClass(TangleFileConsts.k_AppleClassPrefix, key, order, tangled, tangled.Length != 0);

            return appleError;
        }

        static string BuildObfuscatedGooglePlayClass(string googlePlayPublicKey)
        {
            string googleError = null;
            int key = 0;
            int[] order = new int[0];
            byte[] tangled = new byte[0];
            try
            {
                var bytes = Convert.FromBase64String(googlePlayPublicKey);
                order = new int[bytes.Length / 20 + 1];

                tangled = TangleObfuscator.Obfuscate(bytes, order, out key);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Invalid Google Play Public Key. Generating incomplete credentials file. " + e);
                googleError =
                    "  The Google Play License Key is invalid. GooglePlayTangle was generated with incomplete credentials.";
            }
            BuildObfuscatedClass(TangleFileConsts.k_GooglePlayClassPrefix, key, order, tangled, tangled.Length != 0);

            return googleError;
        }


        private static string FullPathForTangleClass(string classnamePrefix)
        {
            return Path.Combine(TangleFileConsts.k_OutputPath, string.Format($"{classnamePrefix}{TangleFileConsts.k_ObfuscationClassSuffix}"));
        }

        internal static bool DoesAppleTangleClassExist()
        {
            return ObfuscatedClassExists(TangleFileConsts.k_AppleClassPrefix);
        }

        internal static bool DoesGooglePlayTangleClassExist()
        {
            return ObfuscatedClassExists(TangleFileConsts.k_GooglePlayClassPrefix);
        }

        private static bool ObfuscatedClassExists(string classnamePrefix)
        {
            return File.Exists(FullPathForTangleClass(classnamePrefix));
        }

        private static void BuildObfuscatedClass(string classnamePrefix, int key, int[] order, byte[] data, bool populated)
        {
            Dictionary<string, string> substitutionDictionary = new Dictionary<string, string>()
            {
                {"{NAME}", classnamePrefix.ToString()},
                {"{KEY}", key.ToString()},
                {"{ORDER}", String.Format("{0}",String.Join(",", Array.ConvertAll(order, i => i.ToString())))},
                {"{DATA}", Convert.ToBase64String(data)},
                {"{POPULATED}", populated.ToString().ToLowerInvariant()} // Defaults to XML-friendly values
            };

            string templateRelativePath = null;
            string templateText = LoadTemplateText(out templateRelativePath);

            if (templateText != null)
            {
                string outfileText = templateText;

                // Apply the parameters to the template
                foreach (var pair in substitutionDictionary)
                {
                    outfileText = outfileText.Replace(pair.Key, pair.Value);
                }
                Directory.CreateDirectory (TangleFileConsts.k_OutputPath);
                File.WriteAllText(FullPathForTangleClass(classnamePrefix), outfileText);
            }
        }

        /// <summary>
        /// Loads the template file.
        /// </summary>
        /// <returns>The template file's text.</returns>
        /// <param name="templateRelativePath">Relative Assets/ path to template file.</param>
        private static string LoadTemplateText(out string templateRelativePath)
        {
            string[] assetGUIDs =
                AssetDatabase.FindAssets(m_GeneratedCredentialsTemplateFilenameNoExtension);
            string templateGUID = null;
            templateRelativePath = null;

            if (assetGUIDs.Length > 0)
            {
                templateGUID = assetGUIDs[0];
            }
            else
            {
                Debug.LogError(String.Format("Could not find template \"{0}\"",
                    m_GeneratedCredentialsTemplateFilename));
            }

            string templateText = null;

            if (templateGUID != null)
            {
                templateRelativePath = AssetDatabase.GUIDToAssetPath(templateGUID);

                string templateAbsolutePath =
                    System.IO.Path.GetDirectoryName(Application.dataPath)
                    + System.IO.Path.DirectorySeparatorChar
                    + templateRelativePath;

                templateText = System.IO.File.ReadAllText(templateAbsolutePath);
            }

            return templateText;
        }
    }
}
