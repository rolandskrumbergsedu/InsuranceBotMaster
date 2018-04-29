using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation
{
    [Serializable]
    public class AutoGlassDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Da kan du henvende deg direkte til en av våre partnere. Du trenger ikke melde skaden til oss.");
            await context.PostAsync("Klikk her for å finne ditt nærmeste verksted! https://www.if.no/privat/meld-skade/bilglass");
            context.Done(this);
        }
    }
}