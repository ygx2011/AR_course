#if UNITY_2018_1_OR_NEWER && UNITY_IOS
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace UnityEngine.Advertisements.Editor.Tests {
    public class SkAdNetworkSourceTests {
        [Test]
        [TestCase("ExpectedPath/To/File")]
        public void LocalSourcePathSetOnCreate(string expectedPath) {
            Assert.That(new SkAdNetworkLocalSource(expectedPath).Path, Is.EqualTo(expectedPath), "Path was not properly set in constructor");
        }

        [Test]
        [TestCase("ExpectedPath/To/File")]
        public void RemoteSourcePathSetOnCreate(string expectedPath) {
            Assert.That(new SkAdNetworkRemoteSource(expectedPath).Path, Is.EqualTo(expectedPath), "Path was not properly set in constructor");
        }

        [Test]
        [TestCase(SkAdNetworkFileExtension.XML, 0)]
        [TestCase(SkAdNetworkFileExtension.JSON, 0)]
        [TestCase(SkAdNetworkFileExtension.NONE, 1)]
        public void LocalSourceProviderFindsExpectedFiles(string extension, int expectedCount) {
            Assert.That(new SkAdNetworkLocalSourceProvider().GetSources("SKAdNetworks", extension).Count(), Is.EqualTo(expectedCount), "GetSources() did not return the expected count");
        }
    }
}
#endif
