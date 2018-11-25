using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InsuranceBotMaster.Translation.Tests
{
    [TestClass]
    public class TranslatorTests
    {
        [TestMethod]
        public async Task Test_Translator()
        {
            var key = "5e6a1347297a4598b815fc3d0a85927e";

            var translator = new Translator(key);

            var result = await translator.TranslateFromNorwegianToEnglish("Mitt navn er Rolands");

            Assert.AreEqual("My name is Rolands", result);
        }
    }
}
