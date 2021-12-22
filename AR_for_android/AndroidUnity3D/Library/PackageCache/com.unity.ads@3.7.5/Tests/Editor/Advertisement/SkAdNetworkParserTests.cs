#if UNITY_2018_1_OR_NEWER && UNITY_IOS
using NUnit.Framework;
using UnityEngine;

namespace UnityEngine.Advertisements.Editor.Tests {
    public class SkAdNetworkParserTests {
        [Test]
        [TestCase(SkAdNetworkFileExtension.XML)]
        [TestCase(SkAdNetworkFileExtension.JSON)]
        [TestCase(SkAdNetworkFileExtension.NONE)]
        public void ParserHandlesExtensionType(string parserType) {
            Assert.That(SkAdNetworkParser.GetParser(parserType).GetExtension(), Is.EqualTo(parserType), "GetExtension did not match the expected value");
        }

        [Test]
        [TestCase("-1")]
        [TestCase(null)]
        [TestCase("a")]
        [TestCase("xmls")]
        [TestCase("xml*")]
        [TestCase("*xml")]
        [TestCase(".xml")]
        public void InvalidParserType(string parserType) {
            Assert.IsNull(SkAdNetworkParser.GetParser(parserType));
        }
    }
}
#endif
