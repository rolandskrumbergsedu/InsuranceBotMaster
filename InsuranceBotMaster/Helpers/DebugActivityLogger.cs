using System.Threading.Tasks;
using Microsoft.Bot.Builder.History;
using Microsoft.Bot.Connector;

namespace InsuranceBotMaster.Helpers
{
    public class DebugActivityLogger : IActivityLogger
    {
        public async Task LogAsync(IActivity activity)
        {
            LogHelper.LogMessage(activity);
        }
    }
}