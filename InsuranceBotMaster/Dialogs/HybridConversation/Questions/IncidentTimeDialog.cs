using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using InsuranceBotMaster.Dialogs.GuidedConversation.Common;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.HybridConversation.Questions
{
    [Serializable]
    public class IncidentTimeDialog : IDialog<object>
    {
        private const string OptionMorning = "Mellom kl. 08-16";
        private const string OptionDay = "Mellom kl. 16-22";
        private const string OptionNight = "Mellom kl. 22-08";

        public async Task StartAsync(IDialogContext context)
        {
            var options = new List<string>
            {
                OptionMorning,
                OptionDay,
                OptionNight
            };

            PromptDialog.Choice(
                context: context,
                resume: ChoiceReceivedAsync,
                options: options,
                prompt: "I hvilket tidsrom skjedde ulykken?"
            );
        }

        public async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var answer = await argument;

            switch (answer)
            {
                case OptionMorning:
                    context.Done(this);
                    break;
                case OptionDay:
                    context.Done(this);
                    break;
                case OptionNight:
                    context.Call(
                        new BasicInputTextDialog(
                            "Ok. Kan du angi et mer nøyaktig tidspunkt? Hvis du er usikker holder det å angi hele timer."),
                        CompleteDialogResumeAfter);
                    break;
            }
        }

        private async Task CompleteDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Done(this);
            }
            else
            {
                context.Call(
                    new BasicInputTextDialog(
                        "Ok. Kan du angi et mer nøyaktig tidspunkt? Hvis du er usikker holder det å angi hele timer."),
                    CompleteDialogResumeAfter);
            }
        }
    }
}