using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InsuranceBotMaster.LUIS.Tests
{
    [TestClass]
    public class LuisCallerTests
    {
        [TestMethod]
        public async Task Call_LUIS()
        {
            var appId = "c3b286b3-832e-426b-b9e2-6d849d5a28c6";
            var luisKey = "92c76cdbf18c4d1db7c2e8e995e29ee2";

            var caller = new LuisCaller(appId, luisKey);

            var utterance = "I crashed my car";

            var luisResult = await caller.QueryLuis(utterance);

            Assert.AreEqual(utterance, luisResult.Query);
            Assert.AreEqual("Car.Insurance.Claim", luisResult.TopScoringIntent.Intent);
            Assert.IsTrue(luisResult.TopScoringIntent.Score > 0.9);
        }
    }
}
