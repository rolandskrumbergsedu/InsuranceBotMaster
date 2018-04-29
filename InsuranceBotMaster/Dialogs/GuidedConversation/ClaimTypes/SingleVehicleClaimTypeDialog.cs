using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation.ClaimTypes
{
    [Serializable]
    public class SingleVehicleClaimTypeDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("This is Single Vehicle Claim Type");
            context.Done(this);
        }
    }
}