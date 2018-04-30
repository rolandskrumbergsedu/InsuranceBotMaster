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
    public class DriverDialog : IDialog<object>
    {
        private const string OptionYes = "Ja";
        private const string OptionNo = "Nei";

        private const string OptionYoung = "23 år eller yngre";
        private const string OptionOld = "24 år eller eldre";
        private const string OptionDontKnow = "Vet ikke";

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

                    context.Done(this);

                    break;
                case OptionNo:

                    await context.PostAsync("Siden du ikke kjørte selv trenger vi kontaktinformasjonen til sjåføren.");
                    context.Call(new SimpleInputTextDialog("Hva heter sjåføren?"), DriversNameDialogResumeAfter);

                    break;
            }

            
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
            context.Done(this);
        }
    }
}