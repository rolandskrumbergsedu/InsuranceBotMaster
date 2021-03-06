﻿using System;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.HybridConversation.Common;
using InsuranceBotMaster.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;

namespace InsuranceBotMaster.Dialogs.HybridConversation.Questions
{
    [Serializable]
    public class WasSombedyInjuredDialog : BasicLuisDialog
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(WasSombedyInjuredDialog).Name);

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
            LogHelper.LogLuisResult(result, context.Activity, typeof(WasSombedyInjuredDialog).Name);

            await context.PostAsync("Håper skadene ikke er alvorlige.");
            await context.PostAsync("Siden det har oppstått en personskade trenger vi litt info om den eller de som har blitt skadet, slik at vi kan følge opp dette.");

            await context.PostAsync("Hvem ble skadet?");
            context.Call(new InjuredDialog(), InjuredDialogResumeAfter);
        }

        [LuisIntent("Open.No")]
        public async Task NoIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(WasSombedyInjuredDialog).Name);

            await context.PostAsync("Det var godt å høre.");
            context.Done(false);
        }

        private async Task InjuredDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                await context.PostAsync("Var det noen andre som ble skadet?");
                context.Call(new WasSomebodyElseInjuredDialog(), WasSomebodyElseInjuredDialogResumeAFter);
            }
            else
            {
                await context.PostAsync("Hvem ble skadet?");
                context.Call(new InjuredDialog(), InjuredDialogResumeAfter);
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