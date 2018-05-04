using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using InsuranceBotMaster.Dialogs.HybridConversation.Common;
using InsuranceBotMaster.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;

namespace InsuranceBotMaster.Dialogs.HybridConversation.Questions
{
    [Serializable]
    public class DamagedItemDialog : BasicLuisDialog
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
            await context.PostAsync("Uff da. Da trenger vi å vite litt mer om det.");
            context.Call(new BasicInputTextDialog("Hva ble skadet og hvilke skader ble det?"), DamagedItemDescriptionDialogResumeAfter);
        }

        [LuisIntent("Open.No")]
        public async Task NoIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Ok.");
            context.Done(false);
        }

        private async Task DamagedItemDescriptionDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                await context.PostAsync("Hvem eier det som ble skadet?");
                context.Call(new DamagedItemOwnerDialog(), DamagedItemOwnerDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hva ble skadet og hvilke skader ble det?"), DamagedItemDescriptionDialogResumeAfter);
            }
        }

        private async Task DamagedItemOwnerDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Ok.");
            context.Done(false);
        }
    }
}