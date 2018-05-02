using System;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.HybridConversation.Common;
using InsuranceBotMaster.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;

namespace InsuranceBotMaster.Dialogs.HybridConversation.Questions
{
    [Serializable]
    public class InjuredDialog : BasicLuisDialog
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
            else
            {
                context.Call(new BasicInputTextDialog("Hva heter den som ble skadet?"), InjuredPersonNameDialogResumeAfter);
            }
        }

        [LuisIntent("Open.Driver")]
        public async Task OpenDriverIntent(IDialogContext context, LuisResult result)
        {
            context.Call(new BasicInputTextDialog("Hvilke skader fikk sjåføren?"), DriverDamageDialogResumeAfter);
        }

        private async Task DriverDamageDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Done(false);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hvilke skader fikk sjåføren?"), DriverDamageDialogResumeAfter);
            }
        }

        [LuisIntent("Open.Passenger")]
        public async Task OpenPassengerIntent(IDialogContext context, LuisResult result)
        {
            context.Call(new BasicInputTextDialog("Hva heter den som ble skadet?"), InjuredPersonNameDialogResumeAfter);
        }

        private async Task InjuredPersonNameDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Call(new BasicInputTextDialog("Hvilke skader fikk han/hun?"), InjuredPersonDamageDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hva heter den som ble skadet?"), InjuredPersonNameDialogResumeAfter);
            }
        }

        private async Task InjuredPersonDamageDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Call(new BasicInputTextDialog("Hva er telefonnummeret hans/hennes?"), InjuredPersonPhoneDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hvilke skader fikk han/hun?"), InjuredPersonDamageDialogResumeAfter);
            }
        }

        private async Task InjuredPersonPhoneDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Call(new BasicInputTextDialog("Hva er e-postadressen hans/hennes?"), InjuredPersonEmailDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hva er telefonnummeret hans/hennes?"), InjuredPersonPhoneDialogResumeAfter);
            }
        }

        private async Task InjuredPersonEmailDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Tusen takk.");
            context.Done(false);
        }

        [LuisIntent("Open.OtherPerson")]
        public async Task OpenOtherIntent(IDialogContext context, LuisResult result)
        {
            context.Call(new BasicInputTextDialog("Hva heter den som ble skadet?"), InjuredPersonNameDialogResumeAfter);
        }
    }
}