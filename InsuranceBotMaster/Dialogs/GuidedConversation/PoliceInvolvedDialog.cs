using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation
{
    [Serializable]
    public class PoliceInvolvedDialog : IDialog<object>
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
                prompt: "Var politiet på ulykkesstedet?"
            );
        }

        public async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            await context.PostAsync("Ok.");
            context.Done(this);
        }
    }
}