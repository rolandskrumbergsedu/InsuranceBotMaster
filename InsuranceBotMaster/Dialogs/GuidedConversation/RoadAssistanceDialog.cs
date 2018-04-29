using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation
{
    [Serializable]
    public class RoadAssistanceDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Da kan du henvende deg direkte til en av våre partnere for å få hjelp.");
            await context.PostAsync("Ring 21497199 for å bestille bergingsbil. Hvis du er utenlands ringer du +4721497176.");
            context.Done(this);
        }
    }
}