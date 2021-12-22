using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace UnityEngine.Advertisements.Editor {
    /// <summary>
    /// Responsible for finding all SkAdNetwork files on the local filesystem by searching through the users project directory and all includes packages.
    /// </summary>
    internal class SkAdNetworkLocalSourceProvider {
        private const int k_MaxPackageLookupTimeoutInSeconds = 30;
        private string[] m_PackagePaths;

        public SkAdNetworkLocalSourceProvider() {
            m_PackagePaths = GetAllPackagePaths();
        }

        public IEnumerable<SkAdNetworkLocalSource> GetSources(string filename, string extension) {
            return GetLocalFilePaths(filename, extension).Select(x => new SkAdNetworkLocalSource(x)).ToArray();
        }

        /// <summary>
        /// Finds a file on the local filesystem by looking the project directory, and all package directories
        /// </summary>
        /// <param name="filename">the filename to look for</param>
        /// <param name="fileExtension">the filename extension to look for</param>
        /// <returns>a full path to the file</returns>
        private IEnumerable<string> GetLocalFilePaths(string filename, string fileExtension) {
            return m_PackagePaths
                .Prepend(Directory.GetCurrentDirectory())
                .SelectMany(path => Directory.GetFiles(path, string.IsNullOrEmpty(fileExtension) ? filename : $"{filename}.{fileExtension}" , SearchOption.AllDirectories))
                .ToList();
        }

        /// <summary>
        /// Returns a list of paths to the root folder of each package included in the users project.
        /// These may be in different locations on disk depending on where the package is being stored/cached.
        /// </summary>
        private static string[] GetAllPackagePaths(bool offlineMode = true)
        {
            var list = UnityEditor.PackageManager.Client.List(offlineMode);
            if (list == null) {
                return Array.Empty<string>();
            }

            var timeSpan = TimeSpan.FromSeconds(k_MaxPackageLookupTimeoutInSeconds);
            var startTime = DateTime.Now;
            while (!list.IsCompleted && (DateTime.Now - startTime) < timeSpan) {
                Thread.Sleep(10);
            }

            if (list.Error != null) {
                return Array.Empty<string>();
            }

            return list.Result.Select(packageInfo => packageInfo.assetPath).ToArray();
        }
    }
}
