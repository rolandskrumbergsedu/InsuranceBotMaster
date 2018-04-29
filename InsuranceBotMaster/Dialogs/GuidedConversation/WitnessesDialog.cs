using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation
{
    [Serializable]
    public class WitnessesDialog : IDialog<object>
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
                prompt: "Var det vitner til hendelsen?"
            );
        }

        public async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var answer = await argument;

            switch (answer)
            {
                case OptionYes:
                    await context.PostAsync("Ok. Ta vare på kontaktinformasjonen til vitner hvis du har det. Det kan hende vi trenger å snakke med de senere.");
                    
                    break;
                case OptionNo:
                    await context.PostAsync("Ok.");
                    break;
            }

            context.Done(this);
        }
    }
}