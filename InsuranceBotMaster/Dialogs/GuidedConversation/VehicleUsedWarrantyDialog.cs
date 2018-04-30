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
    public class VehicleUsedWarrantyDialog : IDialog<object>
    {
        private const string OptionYes = "Ja";
        private const string OptionNo = "Nei";
        private const string OptionDontKnow = "Vet ikke";

        public async Task StartAsync(IDialogContext context)
        {
            var options = new List<string>
            {
                OptionYes,
                OptionNo,
                OptionDontKnow
            };

            PromptDialog.Choice(
                context: context,
                resume: ChoiceReceivedAsync,
                options: options,
                prompt: "Har bilen nybil- eller bruktbilgaranti?"
            );
        }

        public async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var answer = await argument;

            switch (answer)
            {
                case OptionYes:
                    context.Call(new SimpleInputTextDialog("Hvilken forhandler ble bilen kjøpt hos?"), CompleteDialogResumeAfter);
                    break;
                case OptionNo:
                    await context.PostAsync("Ok.");
                    context.Done(this);
                    break;
                case OptionDontKnow:
                    await context.PostAsync("Ok.");
                    context.Done(this);
                    break;
            }
        }

        private async Task CompleteDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Ok.");
            context.Done(this);
        }
    }
}