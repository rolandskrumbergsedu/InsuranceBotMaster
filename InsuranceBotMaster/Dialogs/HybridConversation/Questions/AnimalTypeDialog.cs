using System;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.HybridConversation.Common;
using InsuranceBotMaster.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;

namespace InsuranceBotMaster.Dialogs.HybridConversation.Questions
{
    [Serializable]
    public class AnimalTypeDialog : BasicLuisDialog
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            var qnaResult = await QnaHelper.IsQnA(result.Query);

            if (!string.IsNullOrEmpty(qnaResult))
            {
                await context.PostAsync(qnaResult);
                context.Done(true);
            }

            context.Done(false);
        }

        [LuisIntent("Open.AnimalType.Domestic")]
        public async Task OpenDailyUseIntent(IDialogContext context, LuisResult result)
        {
            context.Call(new BasicInputTextDialog("Ok. Hva er skiltnummeret på kjøretøyet?"), CarRegistrationNumberDialogResumeAfter);
        }

        private async Task CarRegistrationNumberDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                await context.PostAsync("Ok.");
                context.Done(false);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Ok. Hva er skiltnummeret på kjøretøyet?"), CarRegistrationNumberDialogResumeAfter);
            }
        }

        [LuisIntent("Open.AnimalType.Wild")]
        public async Task OpenPlaceHomeIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Når man kjører på vilt er det viktig å ivareta det skadde dyret.");
            await context.PostAsync("Derfor lurer vi på om viltnemda eller politiet ble varslet om hendelsen?");

            context.Call(new WasPoliceInvolvedDialog(), WasPoliceInvolvedDialogResumeAfter);
        }

        private async Task WasPoliceInvolvedDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                await context.PostAsync("Ok.");
                context.Done(false);
            }
            else
            {
                context.Call(new WasPoliceInvolvedDialog(), WasPoliceInvolvedDialogResumeAfter);
            }
        }
    }
}