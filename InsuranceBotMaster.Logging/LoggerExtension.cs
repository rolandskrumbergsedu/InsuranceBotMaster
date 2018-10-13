using System;
using System.Collections.Generic;
using System.Configuration;
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

        public static void LogLuisResult(this Logger logger, Dictionary<string, string> logInfo)
        {
            var dbTarget = new SqlDbTarget
            {
                TableName = "LuisResults",
                ConnectionString = ConfigurationManager.ConnectionStrings["LogDatabase"].ConnectionString
            };

            var myEvent = new LogEventInfo(LogLevel.Info, logger.Name, "LuisResult received.") { LoggerName = logger.Name };
            
            myEvent.Properties.Add("LogTimeStamp", DateTime.Now);
            myEvent.Properties.Add("Dialog", logInfo["Dialog"]);
            myEvent.Properties.Add("ConversationId", logInfo["ConversationId"]);
            myEvent.Properties.Add("Sender", logInfo["Sender"]);
            myEvent.Properties.Add("Recipient", logInfo["Recipient"]);
            myEvent.Properties.Add("Query", logInfo["Query"]);
            myEvent.Properties.Add("TopScoringIntent", logInfo["TopScoringIntent"]);
            myEvent.Properties.Add("TopScoringIntentScore", logInfo["TopScoringIntentScore"]);

            if(logInfo.ContainsKey("TopScoringIntent2"))
            {
                myEvent.Properties.Add("TopScoringIntent2", logInfo["TopScoringIntent2"]);
                myEvent.Properties.Add("TopScoringIntent2Score", logInfo["TopScoringIntent2Score"]);
            }

            if (logInfo.ContainsKey("TopScoringIntent3"))
            {
                myEvent.Properties.Add("TopScoringIntent3", logInfo["TopScoringIntent3"]);
                myEvent.Properties.Add("TopScoringIntent3Score", logInfo["TopScoringIntent3Score"]);
            }

            dbTarget.WriteLuisLog(myEvent);

        }

        public static void LogQnaResult(this Logger logger, Dictionary<string, string> logInfo)
        {
            var dbTarget = new SqlDbTarget
            {
                TableName = "QnaResults",
                ConnectionString = ConfigurationManager.ConnectionStrings["LogDatabase"].ConnectionString
            };

            var myEvent = new LogEventInfo(LogLevel.Info, logger.Name, "QnA result received.") { LoggerName = logger.Name };

            myEvent.Properties.Add("LogTimeStamp", DateTime.Now);

            if (logInfo.ContainsKey("ConversationId"))
            {
                myEvent.Properties.Add("ConversationId", logInfo["ConversationId"]);
            }
            if (logInfo.ContainsKey("Sender"))
            {
                myEvent.Properties.Add("Sender", logInfo["Sender"]);
            }
            if (logInfo.ContainsKey("Recipient"))
            {
                myEvent.Properties.Add("Recipient", logInfo["Recipient"]);
            }

            myEvent.Properties.Add("Query", logInfo["Query"]);

            if (logInfo.ContainsKey("TopScoringAnswer"))
            {
                myEvent.Properties.Add("TopScoringAnswer", logInfo["TopScoringAnswer"]);
                myEvent.Properties.Add("TopScoringAnswerScore", double.Parse(logInfo["TopScoringAnswerScore"]));
            }

            if (logInfo.ContainsKey("TopScoringAnswer2"))
            {
                myEvent.Properties.Add("TopScoringAnswer2", logInfo["TopScoringAnswer2"]);
                myEvent.Properties.Add("TopScoringAnswer2Score", double.Parse(logInfo["TopScoringAnswer2Score"]));
            }

            if (logInfo.ContainsKey("TopScoringAnswer3"))
            {
                myEvent.Properties.Add("TopScoringAnswer3", logInfo["TopScoringAnswer3"]);
                myEvent.Properties.Add("TopScoringAnswer3Score", double.Parse(logInfo["TopScoringAnswer3Score"]));
            }

            myEvent.Properties.Add("MissedTreshold", logInfo["MissedTreshold"]);
            myEvent.Properties.Add("Treshold", double.Parse(logInfo["Treshold"]));

            dbTarget.WriteLuisLog(myEvent);

        }

        public static void LogTranslationResult(this Logger logger, string query, string result, string sourceLanguage, string targetLanguage)
        {
            var dbTarget = new SqlDbTarget
            {
                TableName = "TranslationResults",
                ConnectionString = ConfigurationManager.ConnectionStrings["LogDatabase"].ConnectionString
            };

            var myEvent = new LogEventInfo(LogLevel.Info, logger.Name, "Translation result received.") { LoggerName = logger.Name };

            myEvent.Properties.Add("LogTimeStamp", DateTime.Now);

            myEvent.Properties.Add("Query", query);
            myEvent.Properties.Add("Result", result);
            myEvent.Properties.Add("SourceLanguage", sourceLanguage);
            myEvent.Properties.Add("TargetLanguage", targetLanguage);

            dbTarget.WriteTranslationLog(myEvent);
        }

        public static void LogAimlResult(this Logger logger, string query, string result)
        {
            var dbTarget = new SqlDbTarget
            {
                TableName = "AimlResults",
                ConnectionString = ConfigurationManager.ConnectionStrings["LogDatabase"].ConnectionString
            };

            var myEvent = new LogEventInfo(LogLevel.Info, logger.Name, "AIML result received.") { LoggerName = logger.Name };

            myEvent.Properties.Add("LogTimeStamp", DateTime.Now);

            myEvent.Properties.Add("Query", query);
            myEvent.Properties.Add("Result", result);

            dbTarget.WriteTranslationLog(myEvent);
        }

        public static void LogError(this Logger logger, string message, string exception, string stacktrace)
        {
            var dbTarget = new SqlDbTarget
            {
                TableName = "Errors",
                ConnectionString = ConfigurationManager.ConnectionStrings["LogDatabase"].ConnectionString
            };

            var myEvent = new LogEventInfo(LogLevel.Info, logger.Name, message) { LoggerName = logger.Name };

            myEvent.Properties.Add("Message", message);
            myEvent.Properties.Add("Exception", exception);
            myEvent.Properties.Add("Stacktrace", stacktrace);
            myEvent.Properties.Add("LogTimeStamp", DateTime.Now);

            dbTarget.WriteErrorLog(myEvent);
        }
    }
}