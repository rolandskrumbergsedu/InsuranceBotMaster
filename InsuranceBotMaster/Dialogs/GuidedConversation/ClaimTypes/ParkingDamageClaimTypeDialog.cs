using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using InsuranceBotMaster.Dialogs.GuidedConversation.Common;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation.ClaimTypes
{
    [Serializable]
    public class ParkingDamageClaimTypeDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Call(new BaseStartDialog(), BaseStartDialogResumeAfter);
        }

        private async Task BaseStartDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new ParkingDamageCausedDialog(), ParkingDamageCausedDialogResumeAfter);
        }

        private async Task ParkingDamageCausedDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new SimpleInputTextDialog("Hva er det som har skjedd, og hvilke skader har kjøretøyet fått?"), EventDescriptionDialogResumeAfter);
        }

        private async Task EventDescriptionDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new VehicleLocationDialog(), VehicleLocationDialogResumeAfter);
        }

        private async Task VehicleLocationDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new ContactInformationDialog(), ContactInformationDialogResumeAfter);
        }

        private async Task ContactInformationDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new OtherInformationDialog(), OtherInformationDialogResumeAfter);
        }

        private async Task OtherInformationDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync(
                "Takk, da har vi fått skademeldingen din, og vi kommer til å kontakte deg i løpet av første arbeidsdag.");
            context.Done(this);
        }
    }
}