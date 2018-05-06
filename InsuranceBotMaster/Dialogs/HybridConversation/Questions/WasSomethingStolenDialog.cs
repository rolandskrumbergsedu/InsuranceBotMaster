using System;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.HybridConversation.Common;
using InsuranceBotMaster.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;

namespace InsuranceBotMaster.Dialogs.HybridConversation.Questions
{
    [Serializable]
    public class WasSomethingStolenDialog : BasicLuisDialog
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

        [LuisIntent("Open.Yes")]
        public async Task YesIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Ok. Vi anbefaler at du setter opp en tapsliste så snart som mulig (gjenstand/alder/verdi). Vi kommer til å ta kontakt med deg for videre behandling av tapet.");
            context.Done(false);
        }

        [LuisIntent("Open.No")]
        public async Task NoIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Ok.");
            context.Done(false);
        }

        [LuisIntent("Open.DontKnow")]
        public async Task DontKnowIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Ok.");
            context.Done(false);
        }
    }
}