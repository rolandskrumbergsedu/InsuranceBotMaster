using System;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.HybridConversation.Common;
using InsuranceBotMaster.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;

namespace InsuranceBotMaster.Dialogs.HybridConversation.Questions
{
    [Serializable]
    public class WasCarDamagedDialog : BasicLuisDialog
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(WasCarDamagedDialog).Name);

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
            LogHelper.LogLuisResult(result, context.Activity, typeof(WasCarDamagedDialog).Name);

            context.Call(new BasicInputTextDialog("Hvilke skader har kjøretøyet fått?"), CarDamageDialogResumeAfter);

            context.Done(false);
        }

        [LuisIntent("Open.No")]
        public async Task NoIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(WasCarDamagedDialog).Name);

            context.Done(false);
        }

        private async Task CarDamageDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Call(new WhereIsCarNowDialog(), WhereIsCarNowDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hvilke skader har kjøretøyet fått?"), CarDamageDialogResumeAfter);
            }
        }

        private async Task WhereIsCarNowDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Done(false);
            }
            else
            {
                context.Call(new WhereIsCarNowDialog(), WhereIsCarNowDialogResumeAfter);
            }
        }
    }
}