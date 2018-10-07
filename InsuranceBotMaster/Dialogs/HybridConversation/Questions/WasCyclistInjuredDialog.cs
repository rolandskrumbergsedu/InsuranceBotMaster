using System;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.HybridConversation.Common;
using InsuranceBotMaster.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;

namespace InsuranceBotMaster.Dialogs.HybridConversation.Questions
{
    [Serializable]
    public class WasCyclistInjuredDialog : BasicLuisDialog
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(WasCyclistInjuredDialog).Name);

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
            LogHelper.LogLuisResult(result, context.Activity, typeof(WasCyclistInjuredDialog).Name);

            await context.PostAsync("Uff da, det var leit å høre.");
            await context.PostAsync("Siden det har oppstått en personskade trenger vi litt info om den som har blitt skadet, slik at vi kan følge opp dette.");

            context.Call(new BasicInputTextDialog("Hva heter fotgjengeren?"), PedestrianNameDialogResumeAfter);
        }

        [LuisIntent("Open.No")]
        public async Task NoIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(WasCyclistInjuredDialog).Name);

            await context.PostAsync("Det var godt å høre.");
            context.Done(false);
        }

        private async Task PedestrianNameDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Call(new BasicInputTextDialog("Hvilke skader fikk han/hun?"), PedestrianDamageDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hva heter fotgjengeren?"), PedestrianNameDialogResumeAfter);
            }
        }

        private async Task PedestrianDamageDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Call(new BasicInputTextDialog("Hva er telefonnummeret hans/hennes?"), PedestrianPhoneDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hvilke skader fikk han/hun?"), PedestrianDamageDialogResumeAfter);
            }
        }

        private async Task PedestrianPhoneDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Call(new BasicInputTextDialog("Hva er e-postadressen hans/hennes?"), PedestrianEmailDialogResumeAfter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hva er telefonnummeret hans/hennes?"), PedestrianPhoneDialogResumeAfter);
            }
        }

        private async Task PedestrianEmailDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                await context.PostAsync("Var det noen andre som ble skadet?");
                context.Call(new WasSomebodyElseInjuredDialog(), WasSomebodyElseInjuredDialogResumeAFter);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hva er e-postadressen hans/hennes?"), PedestrianEmailDialogResumeAfter);
            }
        }

        private async Task WasSomebodyElseInjuredDialogResumeAFter(IDialogContext context, IAwaitable<object> result)
        {
            var answeredYes = Convert.ToBoolean(await result);

            if (answeredYes)
            {
                await context.PostAsync("Var det noen andre som ble skadet?");
                context.Call(new WasSomebodyElseInjuredDialog(), WasSomebodyElseInjuredDialogResumeAFter);
            }
            else
            {
                context.Done(false);
            }
        }
    }
}