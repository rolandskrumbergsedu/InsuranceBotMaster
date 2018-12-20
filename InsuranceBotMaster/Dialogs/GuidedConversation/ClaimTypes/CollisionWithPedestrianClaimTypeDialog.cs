using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation.ClaimTypes
{
    [Serializable]
    public class CollisionWithPedestrianClaimTypeDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Call(new BaseStartDialog(), BaseStartDialogResumeAfter);
        }

        private async Task BaseStartDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new DriverDialog(), DriverDialogResumeAfter);
        }

        private async Task DriverDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new PedestrianInjuredDialog(), PedestrianInjuredDialogResumeAfter);
        }

        private async Task PedestrianInjuredDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new PoliceInvolvedDialog(), PoliceInvolvedDialogResumeAfter);
        }

        private async Task PoliceInvolvedDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new IncidentTimeDialog(), IncidentTimeDialogResumeAfter);
        }

        private async Task IncidentTimeDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new IncidentDescriptionDialog(), AccidentDescriptionDialogResumeAfter);
        }

        private async Task AccidentDescriptionDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new VehicleDamageDialog(), VehicleDamageDialogResumeAfter);
        }

        private async Task VehicleDamageDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
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