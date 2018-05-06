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
    public class TechnicalFailureClaimTypeDialog : IDialog<object>
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
                context.Call(new BasicInputTextDialog("Hva er det som er galt med bilen?"), WhatIsWrongDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hva var kilometerstanden på skadetidspunktet? Det holder å gi oss et rundt tall, f.eks. til nærmeste 1000 km."), MilageDialogResumeAfter);
            }
        }

        private async Task WhatIsWrongDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Call(new WhereIsCarNowDialog(), WhereIsCarNowDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hva er det som er galt med bilen?"), WhatIsWrongDialogResumeAfter);
            }
        }

        private async Task WhereIsCarNowDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Call(new HaveWarrantyDialog(), HaveWarrantyDialogResumeAfter);
            }
            else
            {
                context.Call(new WhereIsCarNowDialog(), WhereIsCarNowDialogResumeAfter);
            }
        }

        private async Task HaveWarrantyDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                await context.PostAsync("Da er vi snart ferdige! Vi skal bare sjekke at vi har riktig kontaktinformasjon.");
                context.Call(new BasicInputTextDialog("Hvilket telefonnummer kan vi nå deg på?"), PhoneNumberDialogResumeAfter);
            }
            else
            {
                context.Call(new HaveWarrantyDialog(), HaveWarrantyDialogResumeAfter);
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