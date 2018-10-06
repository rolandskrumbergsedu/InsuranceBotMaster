﻿using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using InsuranceBotMaster.Logging;
using InsuranceBotMaster.QnA;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace InsuranceBotMaster.Helpers
{
	public static class QnaHelper
	{
	    public static async Task<string> IsQnA(IAwaitable<IMessageActivity> argument)
	    {
	        var logger = NLog.LogManager.GetCurrentClassLogger();

            var messsage = await argument as Activity;

	        var qna = new QnaCaller(ConfigurationManager.AppSettings["QnaAppId"], ConfigurationManager.AppSettings["QnaAppKey"]);

	        var qnaResult = await qna.Query(messsage?.Text);
            
	        var qnaTopResult = qnaResult.Answers.OrderByDescending(x => x.Score).FirstOrDefault();

	        var threshold = double.Parse(ConfigurationManager.AppSettings["QnaThreshold"]);

	        if (qnaTopResult != null && qnaTopResult.Score > threshold)
	        {
	            logger.LogQnaResult(messsage?.Text, qnaResult, messsage, false, threshold);

                return qnaTopResult.Answer;
	        }

	        logger.LogQnaResult(messsage?.Text, qnaResult, messsage, true, threshold);
            return string.Empty;
	    }

        public static async Task<string> IsQnA(string messsage)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();

            var qna = new QnaCaller(ConfigurationManager.AppSettings["QnaAppId"], ConfigurationManager.AppSettings["QnaAppKey"]);

            var qnaResult = await qna.Query(messsage);

            var qnaTopResult = qnaResult.Answers.OrderByDescending(x => x.Score).FirstOrDefault();

            var threshold = double.Parse(ConfigurationManager.AppSettings["QnaThreshold"]);

            if (qnaTopResult != null && qnaTopResult.Score > threshold)
            {
                logger.LogQnaResult(messsage, qnaResult, false, threshold);

                return qnaTopResult.Answer;
            }

            logger.LogQnaResult(messsage, qnaResult, true, threshold);
            return string.Empty;
        }
    }
}