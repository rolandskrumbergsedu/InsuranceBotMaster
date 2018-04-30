using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation
{
    [Serializable]
    public class ReportedToPoliceDialog : IDialog<object>
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
                prompt: "Har tyveriet blitt meldt til politiet?"
            );
        }

        public async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var answer = await argument;

            switch (answer)
            {
                case OptionYes:
                    await context.PostAsync("Bra, det er viktig at slike hendelser blir politianmeldt!");
                    break;
                case OptionNo:
                    await context.PostAsync("Tyverier skal alltid meldes til politiet. Fint om du gjør det så fort som mulig.");
                    break;
            }

            context.Done(this);
        }
    }
}