using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiffLibrary.Boxes;
using PiffLibrary.Infrastructure;


namespace PiffLibrary.Test.Infrastructure
{
    [TestClass]
    public class BoxStorageTests
    {
        [TestMethod]
        public void BoxStorage_RootChildOk()
        {
            var sut = new BoxStorage();
            sut.Collect();

            var res = sut.FindBox(null, "meta", out var boxType);

            Assert.AreEqual(FindBoxResults.Found, res);
            Assert.AreEqual(typeof(PiffMetadataBox), boxType);
        }


        [TestMethod]
        public void BoxStorage_RootChildUnexpected()
        {
            var sut = new BoxStorage();
            sut.Collect();

            var res = sut.FindBox(null, "avcC", out var boxType);

            Assert.AreEqual(FindBoxResults.Unexpected, res);
            Assert.AreEqual(typeof(PiffAvcConfigurationBox), boxType);
        }


        [TestMethod]
        public void BoxStorage_RootChildUnrecognized()
        {
            var sut = new BoxStorage();
            sut.Collect();

            var res = sut.FindBox(null, "xxxx", out var boxType);

            Assert.AreEqual(FindBoxResults.Unrecognized, res);
            Assert.AreEqual(typeof(PiffCatchAllBox), boxType);
        }


        [TestMethod]
        public void BoxStorage_BoxChildOk()
        {
            var sut = new BoxStorage();
            sut.Collect();

            var res = sut.FindBox(typeof(PiffMetadataBox), "xml ", out var boxType);

            Assert.AreEqual(FindBoxResults.Found, res);
            Assert.AreEqual(typeof(PiffXmlBox), boxType);
        }


        [TestMethod]
        public void BoxStorage_BoxChildUnexpected()
        {
            var sut = new BoxStorage();
            sut.Collect();

            var res = sut.FindBox(typeof(PiffXmlBox), "xml ", out var boxType);

            Assert.AreEqual(FindBoxResults.Unexpected, res);
            Assert.AreEqual(typeof(PiffXmlBox), boxType);
        }


        [TestMethod]
        public void BoxStorage_BoxChildUnrecognized()
        {
            var sut = new BoxStorage();
            sut.Collect();

            var res = sut.FindBox(typeof(PiffMetadataBox), "xxxx", out var boxType);

            Assert.AreEqual(FindBoxResults.Unrecognized, res);
            Assert.AreEqual(typeof(PiffCatchAllBox), boxType);
        }


        [TestMethod]
        public void BoxStorage_BoxChildPrioritized()
        {
            var sut = new BoxStorage();
            sut.Collect();

            // There are two boxes named "hint"
            var res1 = sut.FindBox(typeof(PiffItemReferenceBox), "hint", out var boxType1);

            Assert.AreEqual(FindBoxResults.Found, res1);
            Assert.AreEqual(typeof(PiffItemReferenceItemBox), boxType1);

            var res2 = sut.FindBox(typeof(PiffTrackReferenceBox), "hint", out var boxType2);

            Assert.AreEqual(FindBoxResults.Found, res2);
            Assert.AreEqual(typeof(PiffTrackReferenceHintBox), boxType2);
        }
    }
}
