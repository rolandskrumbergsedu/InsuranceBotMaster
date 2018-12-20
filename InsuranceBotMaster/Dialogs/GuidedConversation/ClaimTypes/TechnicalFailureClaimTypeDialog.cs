using System;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.GuidedConversation.Common;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation.ClaimTypes
{
    [Serializable]
    public class TechnicalFailureClaimTypeDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Call(new BaseStartDialog(), BaseStartDialogResumeAfter);
        }

        private async Task BaseStartDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new SimpleInputTextDialog("Hva er det som er galt med bilen?"), VehicleParkedTimeDialogResumeAfter);
        }

        private async Task VehicleParkedTimeDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new VehicleLocationSimpleDialog(), VehicleLocationSimpleDialogResumeAfter);
        }

        private async Task VehicleLocationSimpleDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new VehicleUsedWarrantyDialog(), VehicleUsedWarrantyDialogResumeAfter);
        }

        private async Task VehicleUsedWarrantyDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
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