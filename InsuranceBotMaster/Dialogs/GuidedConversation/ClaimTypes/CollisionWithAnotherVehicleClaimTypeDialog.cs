﻿using System;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.GuidedConversation.Common;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation.ClaimTypes
{
    [Serializable]
    public class CollisionWithAnotherVehicleClaimTypeDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Call(new BaseStartDialog(), BaseStartDialogResumeAfter);
        }

        private async Task BaseStartDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new DriverAndInjuredDialog(), InjuredPersonDialogResumeAfter);
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
            context.Call(new SimpleInputTextDialog("Takk. Hva er skiltnummeret til det andre kjøretøyet?"), OtherVehicleRegistrationNumberDialogResumeAfter);
        }

        private async Task OtherVehicleRegistrationNumberDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new SimpleInputTextDialog("Ok. Hvilke synlige skader fikk det andre kjøretøyet?"), OtherVehicleDamageDialogResumeAfter);
        }

        private async Task OtherVehicleDamageDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new WitnessesDialog(), WitnessDialogResumeAfter);
        }

        private async Task WitnessDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
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