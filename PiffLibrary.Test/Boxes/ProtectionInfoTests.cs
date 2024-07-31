using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiffLibrary.Boxes;


namespace PiffLibrary.Test.Boxes;


[TestClass]
public class ProtectionInfoTests
{
    [TestMethod]
    public void Prot_GenerateBin()
    {
        var res = PiffProtectionInfo.CreateBinData("test");
        CollectionAssert.AreEqual(new byte[] { 18, 0, 0, 0, 1, 0, 1, 0, 8, 0, 116, 0, 101, 0, 115, 0, 116, 0 }, res);
    }
}
