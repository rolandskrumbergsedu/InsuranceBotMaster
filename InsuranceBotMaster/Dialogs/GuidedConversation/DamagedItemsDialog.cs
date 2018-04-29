using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.GuidedConversation.Common;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation
{
    [Serializable]
    public class DamagedItemsDialog : IDialog<object>
    {
        private const string OptionYes = "Ja";
        private const string OptionNo = "Nei";

        private const string OptionMyself = "Meg selv";
        private const string OptionSomeoneElse = "Andre";

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
                prompt: "Ble det skader på noe annet enn kjøretøyet? F.eks. gjerde, garasjeport e.l."
            );
        }

        public async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var answer = await argument;

            switch (answer)
            {
                case OptionYes:
                    await context.PostAsync("Uff da. Da trenger vi å vite litt mer om det.");
                    context.Call(new SimpleInputTextDialog("Hva ble skadet og hvilke skader ble det?"), DamageDescriptionDialogResumeAfter);
                    break;
                case OptionNo:
                    context.Done(this);
                    break;
            }
        }

        private async Task DamageDescriptionDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var options = new List<string>
            {
                OptionMyself,
                OptionSomeoneElse
            };

            PromptDialog.Choice(
                context: context,
                resume: DamageChoiceReceivedAsync,
                options: options,
                prompt: "Hvem eier det som ble skadet?"
            );
        }

        public async Task DamageChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var answer = await argument;

            switch (answer)
            {
                case OptionMyself:
                    context.Done(this);
                    break;
                case OptionSomeoneElse:
                    context.Call(new SimpleInputTextDialog("Hva er kontaktinformasjonen til eieren (f.eks. navn, e-post, telefon)?"), DamageChoiceDialogResumeAfter);
                    break;
            }
        }

        private async Task DamageChoiceDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(this);
        }
    }
}