using System;
using System.IO;
using UnityEngine;

namespace UnityEditor.Purchasing
{
    internal class ObfuscationMigration
    {
        /// <summary>
        /// Since we are changing the obfuscation files' location, it may be necessary to migrate existing tangle files to the new location.
        /// Also in 2.0.0, a poor choice of new location was used and has been corrected. If that path exists, its contents are to be moved as well.
        /// </summary>
        [InitializeOnLoadMethod]
        internal static void MigrateObfuscations()
        {
            try
            {
                if (CheckPreviousObfuscationFilesExist())
                {
                    MoveObfuscatorFiles(TangleFileConsts.k_PrevOutputPath);
                }
                else if (CheckBadObfuscationFilesExist())
                {
                    MoveObfuscatorFiles(TangleFileConsts.k_BadOutputPath);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private static void MoveObfuscatorFiles(string oldPath)
        {
            Directory.CreateDirectory (TangleFileConsts.k_OutputPath);

            foreach (var prevFile in Directory.GetFiles(oldPath))
            {
                MoveObfuscatorFile(prevFile);
            }
        }

        static void  MoveObfuscatorFile(string file)
        {
            var fileName = Path.GetFileName(file);
            if (fileName.EndsWith(TangleFileConsts.k_ObfuscationClassSuffix))
            {
                var newFile = $"{TangleFileConsts.k_OutputPath}/{fileName}";

                if (!File.Exists(newFile))
                {
                    AssetDatabase.MoveAsset(file, newFile);
                }
            }
        }

        internal static bool CheckPreviousObfuscationFilesExist()
        {
            return (Directory.Exists(TangleFileConsts.k_PrevOutputPath) && (Directory.GetFiles(TangleFileConsts.k_PrevOutputPath).Length > 0));
        }

        internal static bool CheckBadObfuscationFilesExist()
        {
            return (Directory.Exists(TangleFileConsts.k_BadOutputPath) && (Directory.GetFiles(TangleFileConsts.k_BadOutputPath).Length > 0));
        }
    }
}
