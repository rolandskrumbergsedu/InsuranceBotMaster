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
            var kbId = "51a53b34-dc8f-4c2a-899d-2ae926b2d3fb";
            var qnaKey = "d952660440e84528b63bbbb6039b86be";

            var caller = new QnaCaller(kbId, qnaKey);

            var question = "hi";

            var qnaResult = await caller.Query(question);

            Assert.AreEqual("hello", qnaResult.Answers[0].Answer);
            Assert.AreEqual(100, qnaResult.Answers[0].Score);
        }
    }
}
