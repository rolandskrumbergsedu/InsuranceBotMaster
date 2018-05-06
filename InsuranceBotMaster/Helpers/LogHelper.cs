using System;
using InsuranceBotMaster.QnA;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace InsuranceBotMaster.Helpers
{
    public static class LogHelper
    {
        public static void Log(string messageToLog)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info($"Time: {DateTime.Now}, Conversation: {null},From: {null},To: {null}, Message:{messageToLog}, Query: {null}, TopScoringIntent:{null}, TopScoringIntentScore:{null}");
        }

        public static void Log(IActivity activity)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info($"Time: {DateTime.Now}, Conversation: {activity.Conversation.Id},From: {activity.From.Id},To: {activity.Recipient.Id}, Message:{activity.AsMessageActivity()?.Text}, Query: {null}, TopScoringIntent:{null}, TopScoringIntentScore:{null}");
        }

        public static void Log(string messageToLog, IDialogContext context)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            var conversationId = context.MakeMessage().Conversation.Id;
            logger.Info($"Time: {DateTime.Now}, Conversation: {conversationId},From: {null},To: {null}, Message:{messageToLog}, Query: {null}, TopScoringIntent:{null}, TopScoringIntentScore:{null}");
        }

        public static void Log(LuisResult result, IDialogContext context)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            var conversationId = context.MakeMessage().Conversation.Id;
            var resultIntents = JsonConvert.SerializeObject(result);

            logger.Info($"Time: {DateTime.Now}, Conversation: {conversationId},From: {null},To: {null}, Message:{resultIntents}, Query: {result.Query}, TopScoringIntent:{result.TopScoringIntent.Intent}, TopScoringIntentScore:{result.TopScoringIntent.Score}");
        }

        public static void Log(IDialogContext context, QnaQueryResult answer, string utterance)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            var conversationId = context.MakeMessage().Conversation.Id;
            var resultIntents = JsonConvert.SerializeObject(answer);

            logger.Info($"Time: {DateTime.Now}, Conversation: {conversationId},From: {null},To: {null}, Message:{resultIntents}, Query: {utterance}, TopScoringIntent:{null}, TopScoringIntentScore:{null}");
        }
    }
}