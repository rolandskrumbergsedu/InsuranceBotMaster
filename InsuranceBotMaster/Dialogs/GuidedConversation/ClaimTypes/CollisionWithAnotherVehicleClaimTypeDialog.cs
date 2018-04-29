using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation.ClaimTypes
{
    [Serializable]
    public class CollisionWithAnotherVehicleClaimTypeDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("This is Collision with Another vehicle Claim Type");
            context.Done(this);
        }
    }
}