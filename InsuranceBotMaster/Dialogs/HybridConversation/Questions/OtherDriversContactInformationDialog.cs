using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace InsuranceBotMaster.Dialogs.HybridConversation.Questions
{
    [Serializable]
    public class OtherDriversContactInformationDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Call(new BasicInputTextDialog("Hva heter sjåføren?"), OtherDriversNameDialogResumeAfter);
        }

        private async Task OtherDriversNameDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Call(new BasicInputTextDialog("Er sjåføren 24 år eller eldre?"), OtherDriversAgeDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hva heter sjåføren?"), OtherDriversNameDialogResumeAfter);
            }
        }

        private async Task OtherDriversAgeDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Call(new BasicInputTextDialog("Hva er sjåførens telefonnummer?"), OtherDriversPhoneDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Er sjåføren 24 år eller eldre?"), OtherDriversAgeDialogResumeAfter);
            }
        }

        private async Task OtherDriversPhoneDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Call(new BasicInputTextDialog("Hva er sjåførens e-postadresse?"), OtherDriversEmailDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hva er sjåførens telefonnummer?"), OtherDriversPhoneDialogResumeAfter);
            }
        }

        private async Task OtherDriversEmailDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                await context.PostAsync("Takk!");
                context.Done(this);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hva er sjåførens telefonnummer?"), OtherDriversPhoneDialogResumeAfter);
            }
        }
    }
}