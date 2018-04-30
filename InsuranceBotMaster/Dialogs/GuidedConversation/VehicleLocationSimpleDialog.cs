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
    public class VehicleLocationSimpleDialog : IDialog<object>
    {
        private const string OptionStandingHome = "Står hjemme";
        private const string OptionStandingInYard = "På verksted";
        private const string OptionStandingSomewhereElse = "Et annet sted";
        private const string OptionDontKnow = "Vet ikke";

        public async Task StartAsync(IDialogContext context)
        {
            var options = new List<string>
            {
                OptionStandingHome,
                OptionStandingInYard,
                OptionStandingSomewhereElse,
                OptionDontKnow
            };

            PromptDialog.Choice(
                context: context,
                resume: ChoiceReceivedAsync,
                options: options,
                prompt: "Hvor er kjøretøyet nå?"
            );
        }

        public async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var answer = await argument;

            switch (answer)
            {
                case OptionStandingHome:
                    context.Done(this);
                    break;
                case OptionStandingInYard:
                    context.Call(new SimpleInputTextDialog("Ok. Hvilket verksted står bilen på?"),
                        CompleteDialogResumeAfter);
                    break;
                case OptionStandingSomewhereElse:
                    context.Call(new SimpleInputTextDialog("Ok. Hvor da?"), CompleteDialogResumeAfter);
                    break;
                case OptionDontKnow:
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