using System;
using System.Configuration;
using System.Linq;
using InsuranceBotMaster.QnA;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using NLog;

namespace InsuranceBotMaster.Logging
{
    public static class LoggerExtension
    {

        public static void LogConversation(this Logger logger, string conversationId, string sender, string receiver, string message, DateTime timeStamp, string fullMsg)
        {
            var dbTarget = new SqlDbTarget
            {
                TableName = "ConversationLogs",
                ConnectionString = ConfigurationManager.ConnectionStrings["LogDatabase"].ConnectionString
            };

            var myEvent = new LogEventInfo(LogLevel.Info, logger.Name, message) { LoggerName = logger.Name };
            myEvent.Properties.Add("ConversationId", conversationId);
            myEvent.Properties.Add("Sender", sender);
            myEvent.Properties.Add("Recipient", receiver);
            myEvent.Properties.Add("Message", message);
            myEvent.Properties.Add("LogTimeStamp", timeStamp);
            myEvent.Properties.Add("FullMessage", fullMsg);

            dbTarget.WriteConversationLog(myEvent);
        }

        public static void LogGeneral(this Logger logger, string message)
        {
            var dbTarget = new SqlDbTarget
            {
                TableName = "Logs",
                ConnectionString = ConfigurationManager.ConnectionStrings["LogDatabase"].ConnectionString
            };

            var myEvent = new LogEventInfo(LogLevel.Info, logger.Name, message) { LoggerName = logger.Name };

            myEvent.Properties.Add("Message", message);
            myEvent.Properties.Add("LogTimeStamp", DateTime.Now);

            dbTarget.WriteConversationLog(myEvent);
        }

        public static void LogLuisResult(this Logger logger, LuisResult luisResult, IActivity activity, string dialog)
        {
            var dbTarget = new SqlDbTarget
            {
                TableName = "LuisResults",
                ConnectionString = ConfigurationManager.ConnectionStrings["LogDatabase"].ConnectionString
            };

            var myEvent = new LogEventInfo(LogLevel.Info, logger.Name, "LuisResult received.") { LoggerName = logger.Name };

            var orderedIntents = luisResult.Intents.OrderByDescending(x => x.Score).ToList();

            myEvent.Properties.Add("LogTimeStamp", DateTime.Now);
            myEvent.Properties.Add("Dialog", dialog);
            myEvent.Properties.Add("ConversationId", activity.Conversation.Id);
            myEvent.Properties.Add("From", activity.From);
            myEvent.Properties.Add("To", activity.Id);
            myEvent.Properties.Add("Query", luisResult.Query);
            myEvent.Properties.Add("TopScoringIntent", luisResult.TopScoringIntent.Intent);
            myEvent.Properties.Add("TopScoringIntentScore", luisResult.TopScoringIntent.Score);

            if (orderedIntents.Count > 1)
            {
                myEvent.Properties.Add("TopScoringIntent2", orderedIntents[1].Intent);
                myEvent.Properties.Add("TopScoringIntent2Score", orderedIntents[1].Score);
            }
            if (orderedIntents.Count > 2)
            {
                myEvent.Properties.Add("TopScoringIntent3", orderedIntents[2].Intent);
                myEvent.Properties.Add("TopScoringIntent3Score", orderedIntents[2].Intent);
            }

            dbTarget.WriteLuisLog(myEvent);

        }

        public static void LogQnaResult(this Logger logger, string query, QnaQueryResult qnaResult, Activity activity, bool missedTreshold, double treshold)
        {
            var dbTarget = new SqlDbTarget
            {
                TableName = "QnaResults",
                ConnectionString = ConfigurationManager.ConnectionStrings["LogDatabase"].ConnectionString
            };

            var myEvent = new LogEventInfo(LogLevel.Info, logger.Name, "QnA result received.") { LoggerName = logger.Name };

            var answers = qnaResult.Answers.OrderByDescending(x => x.Score).ToList();

            myEvent.Properties.Add("LogTimeStamp", DateTime.Now);
            myEvent.Properties.Add("ConversationId", activity.Conversation.Id);
            myEvent.Properties.Add("From", activity.From);
            myEvent.Properties.Add("To", activity.Id);

            myEvent.Properties.Add("Query", query);

            if (answers.Count > 0)
            {
                myEvent.Properties.Add("TopScoringAnswer", answers[0].Answer);
                myEvent.Properties.Add("TopScoringAnswerScore", answers[0].Score);
            }

            if (answers.Count > 1)
            {
                myEvent.Properties.Add("TopScoringAnswer2", answers[1].Answer);
                myEvent.Properties.Add("TopScoringAnswer2Score", answers[1].Score);
            }

            if (answers.Count > 1)
            {
                myEvent.Properties.Add("TopScoringAnswer3", answers[2].Answer);
                myEvent.Properties.Add("TopScoringAnswer3Score", answers[2].Score);
            }
            
            myEvent.Properties.Add("MissedTreshold", missedTreshold);
            myEvent.Properties.Add("Treshold", treshold);

            dbTarget.WriteLuisLog(myEvent);

        }

        public static void LogQnaResult(this Logger logger, string query, QnaQueryResult qnaResult, bool missedTreshold, double treshold)
        {
            var dbTarget = new SqlDbTarget
            {
                TableName = "QnaResults",
                ConnectionString = ConfigurationManager.ConnectionStrings["LogDatabase"].ConnectionString
            };

            var myEvent = new LogEventInfo(LogLevel.Info, logger.Name, "QnA result received.") { LoggerName = logger.Name };

            var answers = qnaResult.Answers.OrderByDescending(x => x.Score).ToList();

            myEvent.Properties.Add("LogTimeStamp", DateTime.Now);

            myEvent.Properties.Add("Query", query);

            if (answers.Count > 0)
            {
                myEvent.Properties.Add("TopScoringAnswer", answers[0].Answer);
                myEvent.Properties.Add("TopScoringAnswerScore", answers[0].Score);
            }

            if (answers.Count > 1)
            {
                myEvent.Properties.Add("TopScoringAnswer2", answers[1].Answer);
                myEvent.Properties.Add("TopScoringAnswer2Score", answers[1].Score);
            }

            if (answers.Count > 1)
            {
                myEvent.Properties.Add("TopScoringAnswer3", answers[2].Answer);
                myEvent.Properties.Add("TopScoringAnswer3Score", answers[2].Score);
            }

            myEvent.Properties.Add("MissedTreshold", missedTreshold);
            myEvent.Properties.Add("Treshold", treshold);

            dbTarget.WriteLuisLog(myEvent);

        }
    }
}