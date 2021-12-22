using System;
using System.IO;
using Uniject;

namespace UnityEngine.Purchasing {

    /// <summary>
    /// File Reference that can be created with a filename.
    /// The path to the file is constructed via `Application.persistentDataPath` and `Application.cloudProjectId`.
    /// Operations such as Save, Load, and Delete are available.
    /// One use case for this class is to create a file reference to a locally cached store catalog.
    /// </summary>
    ///
    internal class FileReference {
        private string m_FilePath;
        private ILogger m_Logger;

        /// <summary>
        /// Creates the instance of FileReference. Method allows dependency injection to ease testing
        /// by using Interfaces for the logger and util.
        /// </summary>
        /// <returns>The instance.</returns>
        /// <param name="filename">Filename.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="util">Util.</param>
        internal static FileReference CreateInstance(string filename, ILogger logger, IUtil util) {
            try
            {
                var persistentDataPath = Path.Combine(util.persistentDataPath, "Unity");
                var uniquePathSuffix = Path.Combine(util.cloudProjectId, "IAP");
                var cachePath = Path.Combine(persistentDataPath, uniquePathSuffix);
                Directory.CreateDirectory(cachePath);
                var filePath = Path.Combine(cachePath, filename);
                return new FileReference(filePath, logger);
            }
            catch
            {
                // Not all platforms support writing to disk. E.g. tvOS throws exception: "System.UnauthorizedAccessException: Access to the path "/Unity" is denied."
                return null;
            }

        }

        /// <summary>
        /// Creates an instance of the Persist class
        /// Please use use the `CreateInstance` method unless the filepath
        /// cannot be created through UnityEngine.Application
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <param name="logger">Logger.</param>
        internal FileReference(string filePath, ILogger logger) {
            m_FilePath = filePath;
            m_Logger = logger;
        }

        /// <summary>
        /// Save the specified payload on file.
        /// </summary>
        /// <param name="payload">Payload.</param>
        internal void Save(string payload) {
            try {
                File.WriteAllText(m_FilePath, payload);
            } catch (Exception e) {
                m_Logger.LogError("Failed persisting content", e);
            }
        }

        /// <summary>
        /// Load the contents from the file as a string.
        /// </summary>
        /// <returns>String from file</returns>
        internal string Load() {
            try {
                return File.ReadAllText(m_FilePath);
            } catch {
                return null;
            }
        }

        /// <summary>
        /// Deletes the file
        /// </summary>
        internal void Delete() {
            try {
                File.Delete(m_FilePath);
            } catch (Exception e) {
                m_Logger.LogWarning("Failed deleting cached content", e);
            }
        }
    }
}
