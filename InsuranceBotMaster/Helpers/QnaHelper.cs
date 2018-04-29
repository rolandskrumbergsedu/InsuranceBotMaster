using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using InsuranceBotMaster.QnA;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace InsuranceBotMaster.Helpers
{
	public static class QnaHelper
	{
	    public static async Task<string> IsQnA(IAwaitable<IMessageActivity> argument)
	    {
	        var messsage = await argument as Activity;

	        var qna = new QnaCaller(ConfigurationManager.AppSettings["QnaAppId"], ConfigurationManager.AppSettings["QnaAppKey"]);

	        var qnaResult = await qna.Query(messsage?.Text);

	        var qnaTopResult = qnaResult.Answers.OrderByDescending(x => x.Score).FirstOrDefault();

	        var threshold = double.Parse(ConfigurationManager.AppSettings["QnaAppId"]);

	        if (qnaTopResult != null && qnaTopResult.Score > threshold)
	        {
	            return qnaTopResult.Answer;
	        }

	        return string.Empty;
	    }
    }
}