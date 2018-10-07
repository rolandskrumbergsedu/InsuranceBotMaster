using System;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.HybridConversation.Common;
using InsuranceBotMaster.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;

namespace InsuranceBotMaster.Dialogs.HybridConversation.Questions
{
    [Serializable]
    public class VehicleReworkedDialog : BasicLuisDialog
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(VehicleReworkedDialog).Name);

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
            LogHelper.LogLuisResult(result, context.Activity, typeof(VehicleReworkedDialog).Name);

            await context.PostAsync("Takk. Hvor er kjøretøyet nå?");
            context.Call(new WhereIsCarNowDialog(), WhereIsCarNowDialogResumeAfter);

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
                await context.PostAsync("Takk. Hvor er kjøretøyet nå?");
                context.Call(new WhereIsCarNowDialog(), WhereIsCarNowDialogResumeAfter);
            }
        }

        [LuisIntent("Open.No")]
        public async Task NoIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(VehicleReworkedDialog).Name);

            context.Done(false);
        }
    }
}