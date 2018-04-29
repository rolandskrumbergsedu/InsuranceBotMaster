using System;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.GuidedConversation.Common;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation.ClaimTypes
{
    [Serializable]
    public class SingleVehicleClaimTypeDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Call(new SimpleInputTextDialog("Hvilken dato skjedde ulykken?"), IncidentDateDialogResumeAfter);
        }

        private async Task IncidentDateDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new SimpleInputTextDialog("Ok. Hva er skiltnummeret på kjøretøyet?"), RegistrationNumberDialogResumeAfter);
        }

        private async Task RegistrationNumberDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new SimpleInputTextDialog("Hva var kilometerstanden på skadetidspunktet? Skriv det inn i tekstfeltet under. Det holder å gi oss et rundt tall, f.eks. til nærmeste 1000 km."), MilageDialogResumeAfter);
        }

        private async Task MilageDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new InjuredPersonDialog(), InjuredPersonDialogResumeAfter);
        }

        private async Task InjuredPersonDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new SimpleInputTextDialog("Var politiet på ulykkesstedet?"), PoliceInvolvedDialogResumeAfter);
        }

        private async Task PoliceInvolvedDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new IncidentTimeDialog(), IncidentTimeDialogResumeAfter);
        }

        private async Task IncidentTimeDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Kan du forklare hvordan ulykken skjedde?");
            context.Call(new SimpleInputTextDialog("Beskriv gjerne så nøyaktig som mulig - men husk at du vil få mulighet til å gi utfyllende opplysninger når vi tar kontakt med deg hvis du ikke har alle detaljene akkurat nå."), AccidentDescriptionDialogResumeAfter);
        }
        private async Task AccidentDescriptionDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new SimpleInputTextDialog("Takk. Hvilke synlige skader har kjøretøyet fått?"), VehicleDamageDialogResumeAfter);
        }

        private async Task VehicleDamageDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new VehicleLocationDialog(), VehicleLocationDialogResumeAfter);
        }

        private async Task VehicleLocationDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new DamagedItemsDialog(), DamagedItemDialogResumeAfter);
        }

        private async Task DamagedItemDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Ok.");
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