using System;

namespace UnityEngine.Purchasing
{
	/// <summary>
	/// Utility class for comparing Unity versions. This class only compares the major, minor, and patch versions and 
	/// ignores the suffixes (e.g. f2, p3, b1)
	/// </summary>
	internal static class VersionCheck
	{
		/// <summary>
		/// Represents a three-part version number as three ints
		/// </summary>
		internal struct Version
		{
			public int major;
			public int minor;
			public int patch;
		}

		/// <summary>
		/// Returns true if versionA is greater than or equal to versionB
		/// </summary>
		public static bool GreaterThanOrEqual(string versionA, string versionB)
		{
			return !LessThan(versionA, versionB);
		}

		/// <summary>
		/// Returns true if versionA is greater than versionB
		/// </summary>
		public static bool GreaterThan(string versionA, string versionB)
		{
			return !LessThanOrEqual(versionA, versionB);
		}

		/// <summary>
		/// Returns true if versionA is less than versionB
		/// </summary>
		public static bool LessThan(string versionA, string versionB)
		{
			var va = Parse(versionA);
			var vb = Parse(versionB);

			if (va.major > vb.major) {
				return false;
			} else if (va.major < vb.major) {
				return true;
			} else if (va.minor > vb.minor) {
				return false;
			} else if (va.minor < vb.minor) {
				return true;
			} else if (va.patch > vb.patch) {
				return false;
			} else if (va.patch < vb.patch) {
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		/// Returns true if versionA is less than or equal to versionB
		/// </summary>
		public static bool LessThanOrEqual(string versionA, string versionB)
		{
			return LessThan(versionA, versionB) || !LessThan(versionB, versionA);
		}

		/// <summary>
		/// Returns true if versionA is equal to versionB
		/// </summary>
		public static bool Equal(string versionA, string versionB)
		{
			return !LessThan(versionA, versionB) && !LessThan(versionB, versionA);
		}

		/// <summary>
		/// Parse the major version from a version string as an int. If the version is "3.2.1", this function returns 3.
		/// </summary>
		/// <returns>The major version, or 0 if it cannot be parsed</returns>
		public static int MajorVersion(string version)
		{
			return PartialVersion(version, 0);
		}

		/// <summary>
		/// Parse the minor version from a version string as an int. If the version is "3.2.1", this function returns 2.
		/// </summary>
		/// <returns>The minor version, or 0 if it cannot be parsed</returns>
		public static int MinorVersion(string version)
		{
			return PartialVersion(version, 1);
		}

		/// <summary>
		/// Parse the patch version from a version string as an int. If the version is "3.2.1", this function returns 1.
		/// </summary>
		/// <returns>The patch version, or 0 if it cannot be parsed</returns>
		public static int PatchVersion(string version)
		{
			return PartialVersion(version, 2);
		}

		public static Version Parse(string version)
		{
			Version v = new Version();
			v.major = MajorVersion(version);
			v.minor = MinorVersion(version);
			v.patch = PatchVersion(version);
			return v;
		}

		/// <summary>
		/// Retrieve a part of a version number by index
		/// </summary>
		/// <returns>The parsed version, or 0 if the number can't be parsed.</returns>
		private static int PartialVersion(string version, int index)
		{
			// remove suffix
			string[] parts = version.Split(new char[] { 'a', 'b', 'f', 'p' });
			var prefix = parts[0];

			int result = 0;
			string[] versions = prefix.Split('.');
			if (versions.Length < index + 1)
				return result;
			
			int.TryParse(versions[index], out result);
			return result;
		}
	}
}
