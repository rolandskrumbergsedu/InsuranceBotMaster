using InsuranceBotMaster.Dialogs.HybridConversation.Questions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace InsuranceBotMaster.Dialogs.HybridConversation.ClaimTypes
{
    [Serializable]
    public class CollisionWithPedestrianClaimTypeDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Call(new BasicInputTextDialog("Hvilken dato skjedde ulykken?"), IncidentDateDialogResumeAfter);
        }

        private async Task IncidentDateDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var awaitedResult = await result;
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
                await context.PostAsync("Var det du som kjørte?");
                context.Call(new AreYouTheDriverDialog(), AreYouTheDriverDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Ok. Hva er skiltnummeret på kjøretøyet?"), CarRegistrationNumberDialogResumeAfter);
            }
        }

        private async Task AreYouTheDriverDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                // TO DO: continue with conversation
                context.Done(this);
            }
            else
            {
                context.Call(new AreYouTheDriverDialog(), AreYouTheDriverDialogResumeAfter);
            }
        }
    }
}