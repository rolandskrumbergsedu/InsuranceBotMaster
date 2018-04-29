using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace InsuranceBotMaster.Dialogs.GuidedConversation
{
    [Serializable]
    public class MainGuidedDialog : IDialog<object>
    {
        private const string OptionHelp = "Jeg trenger veihjelp/berging";
        private const string OptionGlass = "Jeg må reparere bilglass";
        private const string OptionClaim = "Jeg vil registrere en skade";

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Velkommen til skadesenteret!");

            var options = new List<string>
            {
                OptionHelp,
                OptionGlass,
                OptionClaim
            };

            PromptDialog.Choice(
                context: context,
                resume: ChoiceReceivedAsync,
                options: options,
                prompt: "Hva kan vi hjelpe deg med?"
            );
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
           
        }

        public async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var answer = await argument;

            switch (answer)
            {
                case OptionHelp:
                    context.Call(new RoadAssistanceDialog(), CompleteDialogResumeAfter);
                    break;
                case OptionGlass:
                    context.Call(new AutoGlassDialog(), CompleteDialogResumeAfter);
                    break;
                case OptionClaim:
                    context.Call(new ReportClaimDialog(), CompleteDialogResumeAfter);
                    break;
            }
        }

        private async Task CompleteDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(this);
        }
    }
}