using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using InsuranceBotMaster.Dialogs.HybridConversation.Common;
using InsuranceBotMaster.Dialogs.HybridConversation.Questions;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.HybridConversation.ClaimTypes
{
    [Serializable]
    public class CarTheftClaimTypeDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Call(new BasicInputTextDialog("Hvilken dato skjedde ulykken?"), IncidentDateDialogResumeAfter);
        }

        private async Task IncidentDateDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Call(new BasicInputTextDialog("Ok. Hva er skiltnummeret på kjøretøyet?"), CarRegistrationNumberDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hvilken dato skjedde ulykken?"), IncidentDateDialogResumeAfter);
            }
        }

        private async Task CarRegistrationNumberDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Call(new BasicInputTextDialog("Hva var kilometerstanden på skadetidspunktet? Det holder å gi oss et rundt tall, f.eks. til nærmeste 1000 km."), MilageDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Ok. Hva er skiltnummeret på kjøretøyet?"), CarRegistrationNumberDialogResumeAfter);
            }
        }

        private async Task MilageDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Call(new BasicInputTextDialog("Når ble kjøretøyet sist parkert før tyveriet?"), ParkingDateDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hva var kilometerstanden på skadetidspunktet? Det holder å gi oss et rundt tall, f.eks. til nærmeste 1000 km."), MilageDialogResumeAfter);
            }
        }

        private async Task ParkingDateDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Call(new BasicInputTextDialog("Hvor var kjøretøyet parkert? Oppgi gjerne adresse eller gatenavn."), ParkingLocationDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Når ble kjøretøyet sist parkert før tyveriet?"), ParkingDateDialogResumeAfter);
            }
        }

        private async Task ParkingLocationDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                await context.PostAsync("Var kjøretøyet låst?");
                context.Call(new VehicleLockedDialog(), VehicleLockedDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hvor var kjøretøyet parkert? Oppgi gjerne adresse eller gatenavn."), ParkingLocationDialogResumeAfter);
            }
        }

        private async Task VehicleLockedDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                await context.PostAsync("Er noen av nøklene borte?");
                context.Call(new VehicleKeysGoneDialog(), VehicleKeysGoneDialogResumeAfter);
            }
            else
            {
                await context.PostAsync("Var kjøretøyet låst?");
                context.Call(new VehicleLockedDialog(), VehicleLockedDialogResumeAfter);
            }
        }

        private async Task VehicleKeysGoneDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                await context.PostAsync("Har kjøretøyet blitt gjenfunnet?");
                context.Call(new VehicleReworkedDialog(), VehicleReworkedGoneDialogResumeAfter);
            }
            else
            {
                await context.PostAsync("Er noen av nøklene borte?");
                context.Call(new VehicleKeysGoneDialog(), VehicleKeysGoneDialogResumeAfter);
            }
        }

        private async Task VehicleReworkedGoneDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                await context.PostAsync("Har tyveriet blitt meldt til politiet?");
                context.Call(new WasPoliceInformedDialog(), WasPoliceInformedDialogResumeAfter);
            }
            else
            {
                await context.PostAsync("Har kjøretøyet blitt gjenfunnet?");
                context.Call(new VehicleReworkedDialog(), VehicleReworkedGoneDialogResumeAfter);
            }
        }

        private async Task WasPoliceInformedDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                await context.PostAsync("Da er vi snart ferdige! Vi skal bare sjekke at vi har riktig kontaktinformasjon.");
                context.Call(new BasicInputTextDialog("Hvilket telefonnummer kan vi nå deg på?"), PhoneNumberDialogResumeAfter);
            }
            else
            {
                await context.PostAsync("Har tyveriet blitt meldt til politiet?");
                context.Call(new WasPoliceInformedDialog(), WasPoliceInformedDialogResumeAfter);
            }
        }

        private async Task PhoneNumberDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Call(new BasicInputTextDialog("Hvilken e-post vil du at vi bruker?"), EmailDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hvilket telefonnummer kan vi nå deg på?"), PhoneNumberDialogResumeAfter);
            }
        }

        private async Task EmailDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                await context.PostAsync("Takk!");
                await context.PostAsync("Er det noe mer du vil fortelle om hendelsen eller skadene før vi avslutter?");
                context.Call(new IsSomethingElseToTellDialog(), IsSomethingElseToTellDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hvilken e-post vil du at vi bruker?"), EmailDialogResumeAfter);
            }
        }

        private async Task IsSomethingElseToTellDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Takk, da har vi fått skademeldingen din, og vi kommer til å kontakte deg i løpet av første arbeidsdag.");
            context.Done(this);
        }
    }

}