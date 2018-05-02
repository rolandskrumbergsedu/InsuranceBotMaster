using InsuranceBotMaster.Dialogs.HybridConversation.Questions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace InsuranceBotMaster.Dialogs.HybridConversation.ClaimTypes
{
    [Serializable]
    public class CollisionWithPedestrianClaimTypeDialog : IDialog<object>
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
                await context.PostAsync("Var det du som kjørte?");
                context.Call(new AreYouTheDriverDialog(), AreYouTheDriverDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hva var kilometerstanden på skadetidspunktet? Det holder å gi oss et rundt tall, f.eks. til nærmeste 1000 km."), MilageDialogResumeAfter);
            }
        }

        private async Task AreYouTheDriverDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                await context.PostAsync("Ble fotgjengeren skadet i ulykken?");
                context.Call(new WasPedestrianInjuredDialog(), WasPedestrianInjuredDialogResumeAfter);
            }
            else
            {
                context.Call(new AreYouTheDriverDialog(), AreYouTheDriverDialogResumeAfter);
            }
        }

        private async Task WasPedestrianInjuredDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Call(new WasPoliceInvolvedDialog(), WasPoliceInvolvedDialogResumeAfter);
            }
            else
            {
                await context.PostAsync("Ble fotgjengeren skadet i ulykken?");
                context.Call(new WasPedestrianInjuredDialog(), WasPedestrianInjuredDialogResumeAfter);
            }
        }

        private async Task WasPoliceInvolvedDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Call(new IncidentTimeDialog(), IncidentTimeDialogResumeAfter);
            }
            else
            {
                context.Call(new WasPoliceInvolvedDialog(), WasPoliceInvolvedDialogResumeAfter);
            }
        }

        private async Task IncidentTimeDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                await context.PostAsync("Kan du forklare hvordan ulykken skjedde?");
                context.Call(new BasicInputTextDialog("Beskriv gjerne så nøyaktig som mulig - men husk at hvis du ikke har alle detaljene nå vil du få mulighet til å gi utfyllende opplysninger når vi tar kontakt med deg."), IncidentDescriptionDialogResumeAfter);
            }
            else
            {
                context.Call(new IncidentTimeDialog(), IncidentTimeDialogResumeAfter);
            }
        }

        private async Task IncidentDescriptionDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Call(new WasCarDamagedDialog(), WasCarDamagedDialogResumeAfter);
            }
            else
            {
                await context.PostAsync("Kan du forklare hvordan ulykken skjedde?");
                context.Call(new BasicInputTextDialog("Beskriv gjerne så nøyaktig som mulig - men husk at hvis du ikke har alle detaljene nå vil du få mulighet til å gi utfyllende opplysninger når vi tar kontakt med deg."), IncidentDescriptionDialogResumeAfter);
            }
        }

        private async Task WasCarDamagedDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Call(new WhereIsCarNowDialog(), WhereIsCarNowDialogResumeAfter);
            }
            else
            {
                context.Call(new WasCarDamagedDialog(), WasCarDamagedDialogResumeAfter);
            }
        }

        private async Task WhereIsCarNowDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                await context.PostAsync("Da er vi snart ferdige! Vi skal bare sjekke at vi har riktig kontaktinformasjon.");
                context.Call(new BasicInputTextDialog("Hvilket telefonnummer kan vi nå deg på?"), PhoneNumberDialogResumeAfter);
            }
            else
            {
                context.Call(new WhereIsCarNowDialog(), WhereIsCarNowDialogResumeAfter);
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