using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.GuidedConversation.Common;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation
{
    [Serializable]
    public class DriverAndInjuredDialog : IDialog<object>
    {
        private const string OptionYes = "Ja";
        private const string OptionNo = "Nei";

        private const string OptionYoung = "Ja";
        private const string OptionOld = "Nei";
        private const string OptionDontKnow = "Nei";

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
                prompt: "Var det du som kjørte?"
            );
        }

        public async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var answer = await argument;

            switch (answer)
            {
                case OptionYes:

                    var options = new List<string>
                    {
                        OptionYes,
                        OptionNo
                    };

                    PromptDialog.Choice(
                        context: context,
                        resume: InjuredChoiceReceivedAsync,
                        options: options,
                        prompt: "Ble noen personer skadet i ulykken?"
                    );

                    break;
                case OptionNo:

                    await context.PostAsync("Siden du ikke kjørte selv trenger vi kontaktinformasjonen til sjåføren.");
                    context.Call(new SimpleInputTextDialog("Hva heter sjåføren?"), DriversNameDialogResumeAfter);

                    break;
            }

            context.Done(this);
        }

        private async Task DriversNameDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var options = new List<string>
            {
                OptionYoung,
                OptionOld,
                OptionDontKnow
            };

            PromptDialog.Choice(
                context: context,
                resume: DriversAgeChoiceReceivedAsync,
                options: options,
                prompt: "Og hvor gammel er sjåføren?"
            );
        }

        public async Task DriversAgeChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            context.Call(new SimpleInputTextDialog("Hva er sjåførens telefonnummer?"), DriversPhoneDialogResumeAfter);
        }

        private async Task DriversPhoneDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new SimpleInputTextDialog("Hva er sjåførens e-postadresse?"), DriversEmailDialogResumeAfter);
        }

        private async Task DriversEmailDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Takk!");

            var options = new List<string>
            {
                OptionYes,
                OptionNo
            };

            PromptDialog.Choice(
                context: context,
                resume: InjuredChoiceReceivedAsync,
                options: options,
                prompt: "Ble noen personer skadet i ulykken?"
            );
        }

        public async Task InjuredChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var answer = await argument;

            switch (answer)
            {
                case OptionYes:

                    await context.PostAsync("Håper skadene ikke er alvorlige.");
                    await context.PostAsync("Siden det har oppstått en personskade trenger vi litt info om den eller de som har blitt skadet, slik at vi kan følge opp dette.");

                    context.Call(new InjuredDialog(), InjuredDialogResumeAfter);

                    break;
                case OptionNo:

                    await context.PostAsync("Det var godt å høre.");
                    context.Done(this);

                    break;
            }
        }

        private async Task InjuredDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var options = new List<string>
            {
                OptionYes,
                OptionNo
            };

            PromptDialog.Choice(
                context: context,
                resume: InjuredMoreChoiceReceivedAsync,
                options: options,
                prompt: "Var det noen andre personer som ble skadet?"
            );
        }

        private async Task InjuredMoreChoiceReceivedAsync(IDialogContext context, IAwaitable<string> result)
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
    }
}