using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using InsuranceBotMaster.Dialogs.GuidedConversation.Common;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation
{
    [Serializable]
    public class InjuredDialog : IDialog<object>
    {
        private const string OptionDriver = "Sjåføren";
        private const string OptionPassenger = "Passasjer";
        private const string OptionOther = "Annen person";

        public async Task StartAsync(IDialogContext context)
        {
            var options = new List<string>
            {
                OptionDriver,
                OptionPassenger,
                OptionOther
            };

            PromptDialog.Choice(
                context: context,
                resume: ChoiceReceivedAsync,
                options: options,
                prompt: "Hvem ble skadet?"
            );
        }
        public async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var answer = await argument;

            switch (answer)
            {
                case OptionDriver:
                    context.Call(new SimpleInputTextDialog("Hvilke skader fikk sjåføren?"), DriversDamageDialogResumeAfter);
                    break;
                case OptionPassenger:
                    context.Call(new SimpleInputTextDialog("Hva heter den som ble skadet?"), PassengerDamageDialogResumeAfter);
                    break;
                case OptionOther:
                    context.Call(new SimpleInputTextDialog("Hva heter den som ble skadet?"), PassengerDamageDialogResumeAfter);
                    break;
            }

            context.Done(this);
        }

        private async Task PassengerDamageDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new SimpleInputTextDialog("Hvilke skader fikk han/hun?"), PassengerNameDialogResumeAfter);
        }

        private async Task PassengerNameDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new SimpleInputTextDialog("Hva er telefonnummeret hans/hennes?"), PassengerPhoneDialogResumeAfter);
        }

        private async Task PassengerPhoneDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new SimpleInputTextDialog("Hva er e-postadressen hans/hennes?"), PassengerEmailDialogResumeAfter);
        }

        private async Task PassengerEmailDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(this);
        }

        private async Task DriversDamageDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(this);
        }
    }
}