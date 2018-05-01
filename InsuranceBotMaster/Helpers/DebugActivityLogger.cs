using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.History;
using Microsoft.Bot.Connector;

namespace InsuranceBotMaster.Helpers
{
    public class DebugActivityLogger : IActivityLogger
    {
        public async Task LogAsync(IActivity activity)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info($"Time: {DateTime.Now}, Conversation: {activity.Conversation.Id},From: {activity.From.Id},To: {activity.Recipient.Id}, Message:{activity.AsMessageActivity()?.Text}");
        }
    }
}