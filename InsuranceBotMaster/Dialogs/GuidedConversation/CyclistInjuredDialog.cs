﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using InsuranceBotMaster.Dialogs.GuidedConversation.Common;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation
{
    [Serializable]
    public class CyclistInjuredDialog : IDialog<object>
    {
        private const string OptionYes = "Ja";
        private const string OptionNo = "Nei";

        public async Task StartAsync(IDialogContext context)
        {
            var options = new List<string>
            {
                OptionYes,
                OptionNo
            };

            PromptDialog.Choice(
                context: context,
                resume: ChoiceReceivedAsync,
                options: options,
                prompt: "Ble syklisten skadet  i ulykken?"
            );
        }

        public async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var answer = await argument;

            switch (answer)
            {
                case OptionYes:
                    await context.PostAsync("Uff da, det var leit å høre.");
                    await context.PostAsync("Siden det har oppstått en personskade trenger vi litt info om den som har blitt skadet, slik at vi kan følge opp dette.");

                    context.Call(new SimpleInputTextDialog("Hva heter syklisten?"), CyclistNameDialogResumeAfter);

                    break;
                case OptionNo:
                    await context.PostAsync("Det var godt å høre.");
                    await SomebodyElseInjured(context);

                    break;
            }

        }

        private async Task CyclistNameDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new SimpleInputTextDialog("Hvilke skader fikk han/hun?"), CyclistDamageDialogResumeAfter);
        }

        private async Task CyclistDamageDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new SimpleInputTextDialog("Hva er telefonnummeret hans/hennes?"), CyclistPhoneDialogResumeAfter);
        }

        private async Task CyclistPhoneDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new SimpleInputTextDialog("Hva er e-postadressen hans/hennes?"), CyclistEmailDialogResumeAfter);
        }

        private async Task CyclistEmailDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await SomebodyElseInjured(context);
        }

        private async Task SomebodyElseInjured(IDialogContext context)
        {
            var options = new List<string>
            {
                OptionYes,
                OptionNo
            };

            PromptDialog.Choice(
                context: context,
                resume: OtherInjuredChoiceReceivedAsync,
                options: options,
                prompt: "Var det noen andre som ble skadet?"
            );
        }

        private async Task OtherInjuredChoiceReceivedAsync(IDialogContext context, IAwaitable<string> result)
        {
            var answer = await result;

            switch (answer)
            {
                case OptionYes:
                    context.Call(new InjuredDialog(), InjuredDialogResumeAfter);
                    break;
                case OptionNo:
                    context.Done(this);

                    break;
            }
        }

        private async Task InjuredDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(this);
        }
    }
}