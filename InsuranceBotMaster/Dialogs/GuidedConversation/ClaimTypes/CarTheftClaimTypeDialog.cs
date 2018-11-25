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
    public class CarTheftClaimTypeDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Call(new BaseStartDialog(), BaseStartDialogResumeAfter);
        }

        private async Task BaseStartDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new SimpleInputTextDialog("Når ble kjøretøyet sist parkert før tyveriet?"), VehicleParkedTimeDialogResumeAfter);
        }

        private async Task VehicleParkedTimeDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new SimpleInputTextDialog("Hvor var kjøretøyet parkert? Oppgi gjerne adresse eller gatenavn."), VehicleParkedPlaceDialogResumeAfter);
        }

        private async Task VehicleParkedPlaceDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new VehicleLockedDialog(), VehicleLockedDialogResumeAfter);
        }

        private async Task VehicleLockedDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new VehicleKeysGoneDialog(), VehicleKeysGoneDialogResumeAfter);
        }

        private async Task VehicleKeysGoneDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new VehicleFoundDialog(), VehicleFoundDialogResumeAfter);
        }

        private async Task VehicleFoundDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new ReportedToPoliceDialog(), ReportedToPoliceDialogResumeAfter);
        }

        private async Task ReportedToPoliceDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
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
            context.EndConversation("End of conversation");
        }
    }
}