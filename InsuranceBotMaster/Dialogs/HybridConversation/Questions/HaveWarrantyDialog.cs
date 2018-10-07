using System;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.HybridConversation.Common;
using InsuranceBotMaster.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;

namespace InsuranceBotMaster.Dialogs.HybridConversation.Questions
{
    [Serializable]
    public class HaveWarrantyDialog : BasicLuisDialog
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(HaveWarrantyDialog).Name);

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
            LogHelper.LogLuisResult(result, context.Activity, typeof(HaveWarrantyDialog).Name);

            context.Call(new BasicInputTextDialog("Hva er det som er galt med bilen?"), WherePurchasedDialogResumeAfter);
        }

        [LuisIntent("Open.No")]
        public async Task NoIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(HaveWarrantyDialog).Name);

            await context.PostAsync("Ok.");
            context.Done(false);
        }

        [LuisIntent("Open.DontKnow")]
        public async Task DontKnowIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(HaveWarrantyDialog).Name);

            await context.PostAsync("Ok.");
            context.Done(false);
        }

        private async Task WherePurchasedDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(false);
        }
    }
}