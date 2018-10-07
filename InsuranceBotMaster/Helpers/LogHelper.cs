using System.Collections.Generic;
using System.Linq;
using InsuranceBotMaster.QnA;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using InsuranceBotMaster.Logging;
using NLog;
using System;

namespace InsuranceBotMaster.Helpers
{
    public static class LogHelper
    {
        public static void LogMessage(IActivity activity)
        {
            var logger = LogManager.GetCurrentClassLogger();

            logger.LogConversation(activity.Conversation.Id, activity.From.Id, activity.Recipient.Id, activity.AsMessageActivity()?.Text, DateTime.Now, "");
        }

        public static void LogLuisResult(LuisResult luisResult, IActivity activity, string dialog)
        {
            var logger = LogManager.GetCurrentClassLogger();

            var orderedIntents = luisResult.Intents.OrderByDescending(x => x.Score).ToList();

            var result = new Dictionary<string, string>
            {
                { "Dialog", dialog },
                { "ConversationId", activity.Conversation.Id},
                { "From", activity.From.Id },
                { "To", activity.Recipient.Id },
                { "Query", luisResult.Query },
                { "TopScoringIntent", luisResult.TopScoringIntent.Intent },
                { "TopScoringIntentScore", luisResult.TopScoringIntent.Score.ToString() }
            };

            if (orderedIntents.Count > 1)
            {
                result.Add("TopScoringIntent2", orderedIntents[1].Intent);
                result.Add("TopScoringIntent2Score", orderedIntents[1].Score.ToString());
            }
            if (orderedIntents.Count > 2)
            {
                result.Add("TopScoringIntent3", orderedIntents[2].Intent);
                result.Add("TopScoringIntent3Score", orderedIntents[2].Score.ToString());
            }

            logger.LogLuisResult(result);
        }

        public static void LogQnaResult(string query, QnaQueryResult qnaResult, Activity activity, bool missedTreshold, double treshold)
        {
            var logger = LogManager.GetCurrentClassLogger();

            var result = new Dictionary<string, string>();

            var answers = qnaResult.Answers.OrderByDescending(x => x.Score).ToList();
            
            result.Add("ConversationId", activity.Conversation.Id);
            result.Add("From", activity.From.Id);
            result.Add("To", activity.Recipient.Id);

            result.Add("Query", query);

            if (answers.Count > 0)
            {
                result.Add("TopScoringAnswer", answers[0].Answer);
                result.Add("TopScoringAnswerScore", answers[0].Score.ToString());
            }

            if (answers.Count > 1)
            {
                result.Add("TopScoringAnswer2", answers[1].Answer);
                result.Add("TopScoringAnswer2Score", answers[1].Score.ToString());
            }

            if (answers.Count > 1)
            {
                result.Add("TopScoringAnswer3", answers[2].Answer);
                result.Add("TopScoringAnswer3Score", answers[2].Score.ToString());
            }

            result.Add("MissedTreshold", missedTreshold.ToString());
            result.Add("Treshold", treshold.ToString());

            logger.LogQnaResult(result);

        }

        public static void LogQnaResult(string query, QnaQueryResult qnaResult, bool missedTreshold, double treshold)
        {
            var logger = LogManager.GetCurrentClassLogger();

            var result = new Dictionary<string, string>();

            var answers = qnaResult.Answers.OrderByDescending(x => x.Score).ToList();
            
            result.Add("Query", query);

            if (answers.Count > 0)
            {
                result.Add("TopScoringAnswer", answers[0].Answer);
                result.Add("TopScoringAnswerScore", answers[0].Score.ToString());
            }

            if (answers.Count > 1)
            {
                result.Add("TopScoringAnswer2", answers[1].Answer);
                result.Add("TopScoringAnswer2Score", answers[1].Score.ToString());
            }

            if (answers.Count > 1)
            {
                result.Add("TopScoringAnswer3", answers[2].Answer);
                result.Add("TopScoringAnswer3Score", answers[2].Score.ToString());
            }

            result.Add("MissedTreshold", missedTreshold.ToString());
            result.Add("Treshold", treshold.ToString());

            logger.LogQnaResult(result);
        }
    }
}