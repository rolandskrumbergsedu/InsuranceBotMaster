using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using InsuranceBotMaster.Dialogs.GuidedConversation.Common;
using Microsoft.Bot.Builder.Dialogs;
using NLog.Web.LayoutRenderers;

namespace InsuranceBotMaster.Dialogs.GuidedConversation
{
    [Serializable]
    public class OtherInformationDialog : IDialog<object>
    {
        private const string OptionYes = "Ja";
        private const string OptionNo = "Neis";

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
                prompt: "Er det noe mer du vil fortelle om hendelsen eller skadene før vi avslutter?"
            );
        }

        public async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var answer = await argument;

            switch (answer)
            {
                case OptionYes:
                    context.Call(new SimpleInputTextDialog("Ok, hva vil du legge til?"), CompleteDialogResumeAfter);
                    break;
                case OptionNo:
                    context.Done(this);
                    break;
            }
        }

        private async Task CompleteDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(this);
        }
    }
}