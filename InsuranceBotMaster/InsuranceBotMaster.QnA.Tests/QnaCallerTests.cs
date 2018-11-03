using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InsuranceBotMaster.QnA.Tests
{
    [TestClass]
    public class QnaCallerTests
    {
        [TestMethod]
        public async Task Call_QnA()
        {
            var kbId = "13b548dd-e2c7-4513-b331-237511233965";
            var qnaKey = "93a2ad45-07b9-4032-beb8-39902fd9a525";

            var caller = new QnaCaller(kbId, qnaKey);

            var question = "hi";

            var qnaResult = await caller.Query(question);

            Assert.AreEqual("Hello!", qnaResult.Answers[0].Answer);
            Assert.AreEqual(100, qnaResult.Answers[0].Score);
        }
    }
}
