using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.HybridConversation.Questions
{
    public class MotorNoDriverAndInjuredDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Var det du som kjørte?");
            context.Call(new AreYouTheDriverDialog(), AreYouTheDriverDialogResumeAfter);
        }

        private async Task AreYouTheDriverDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                await context.PostAsync("Ble noen personer skadet i ulykken?");
                context.Call(new WasSombedyInjuredDialog(), WasSombedyInjuredDialogResumeAfter);
            }
            else
            {
                context.Call(new AreYouTheDriverDialog(), AreYouTheDriverDialogResumeAfter);
            }
        }

        private async Task WasSombedyInjuredDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Done(false);
            }
            else
            {
                context.Call(new WasSombedyInjuredDialog(), WasSombedyInjuredDialogResumeAfter);
            }
        }
    }
}