using InsuranceBotMaster.Dialogs.HybridConversation.Common;
using InsuranceBotMaster.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Threading.Tasks;

namespace InsuranceBotMaster.Dialogs.HybridConversation.Questions
{
    [Serializable]
    public class AreYouTheDriverDialog : BasicLuisDialog
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
        }

        [LuisIntent("Open.Yes")]
        public async Task YesIntent(IDialogContext context, LuisResult result)
        {
            context.Done(false);
        }

        [LuisIntent("Open.No")]
        public async Task NoIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Siden du ikke kjørte selv trenger vi kontaktinformasjonen til sjåføren.");

            context.Call(new OtherDriversContactInformationDialog(), OtherDriversContactInformationDialogResumeAfter);
        }

        private async Task OtherDriversContactInformationDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(false);
        }
    }
}