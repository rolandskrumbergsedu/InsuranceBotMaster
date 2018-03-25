using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InsuranceBotMaster.AIML.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AimlEngine_Can_Be_Instantiated()
        {
            var bot  = new Bot();
            bot.LoadSettings();
            bot.LoadAIMLFromFiles();
            Result output = bot.Chat("bye", "1");
            Assert.AreEqual("Cheerio.", output.RawOutput);
        }
    }
}
