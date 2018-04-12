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
            var key = "29569f3847a84fa3ba411afdc2618e1e";

            var translator = new Translator(key);

            var result = await translator.TranslateFromEnglishToNorwegian("My name is Rolands");
        }
    }
}
