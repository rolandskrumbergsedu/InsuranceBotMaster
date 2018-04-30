using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.GuidedConversation.Common;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation
{
    [Serializable]
    public class AnimalTypeDialog : IDialog<object>
    {
        private const string OptionWild = "Vilt";
        private const string OptionDomestic = "Husdyr";

        private const string OptionYes = "Ja";
        private const string OptionNo = "Nei";

        public async Task StartAsync(IDialogContext context)
        {
            var options = new List<string>
            {
                OptionWild,
                OptionDomestic
            };

            PromptDialog.Choice(
                context: context,
                resume: ChoiceReceivedAsync,
                options: options,
                prompt: "Hva slags dyr var det som ble påkjørt?"
            );
        }

        public async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var answer = await argument;

            switch (answer)
            {
                case OptionWild:
                    await context.PostAsync("Når man kjører på vilt er det viktig å ivareta det skadde dyret.");

                    var options = new List<string>
                    {
                        OptionYes,
                        OptionNo
                    };

                    PromptDialog.Choice(
                        context: context,
                        resume: AnimalMovedReceivedAsync,
                        options: options,
                        prompt: "Var det vitner til hendelsen?"
                    );

                    break;
                case OptionDomestic:

                    context.Call(new SimpleInputTextDialog("Hva er telefonnummeret til eieren?"), OwnersPhoneDialogResumeAfter);

                    break;
            }
        }

        private async Task AnimalMovedReceivedAsync(IDialogContext context, IAwaitable<string> result)
        {
            await context.PostAsync("Ok.");
            context.Done(this);
        }

        private async Task OwnersPhoneDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Ok.");
            context.Done(this);
        }
    }
}