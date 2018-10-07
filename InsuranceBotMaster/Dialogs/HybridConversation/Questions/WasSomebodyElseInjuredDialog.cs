using System;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.HybridConversation.Common;
using InsuranceBotMaster.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;

namespace InsuranceBotMaster.Dialogs.HybridConversation.Questions
{
    [Serializable]
    public class WasSomebodyElseInjuredDialog : BasicLuisDialog
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(WasSomebodyElseInjuredDialog).Name);

            var qnaResult = await QnaHelper.IsQnA(result.Query);

            if (!string.IsNullOrEmpty(qnaResult))
            {
                await context.PostAsync(qnaResult);
                context.Done(false);
            }

            context.Done(false);
        }

        [LuisIntent("Open.Yes")]
        public async Task YesIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(WasSomebodyElseInjuredDialog).Name);

            await context.PostAsync("Uff da, det var leit å høre.");
            await context.PostAsync("Hvem ble skadet?");
            context.Call(new InjuredDialog(), InjuredDialogResumeAfter);
        }

        [LuisIntent("Open.No")]
        public async Task NoIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(WasSomebodyElseInjuredDialog).Name);

            context.Done(false);
        }

        private async Task InjuredDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(true);
        }
    }
}