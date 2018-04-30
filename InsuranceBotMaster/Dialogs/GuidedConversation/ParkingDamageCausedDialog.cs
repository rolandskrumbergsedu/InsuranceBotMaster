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
    public class ParkingDamageCausedDialog : IDialog<object>
    {
        private const string OptionOtherVehicle = "Annet kjøretøy";
        private const string OptionOther = "På verksted";
        private const string OptionUnknown = "Nei, ukjent årsak";

        public async Task StartAsync(IDialogContext context)
        {
            var options = new List<string>
            {
                OptionOtherVehicle,
                OptionOther,
                OptionUnknown
            };

            PromptDialog.Choice(
                context: context,
                resume: ChoiceReceivedAsync,
                options: options,
                prompt: "Vet du hvem eller hva som har forårsaket skaden?"
            );
        }

        public async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var answer = await argument;

            switch (answer)
            {
                case OptionOtherVehicle:
                    context.Call(new SimpleInputTextDialog("Ok. Oppgi registreringsnummeret på kjøretøyet som forårsaket skaden hvis du har dette."), CompleteDialogResumeAfter);
                    break;
                case OptionOther:
                    context.Call(new SimpleInputTextDialog("Ok, oppgi skadevolders navn og kontaktinfo (tlf og/eller e-post) hvis du har dette."), CompleteDialogResumeAfter);
                    break;
                case OptionUnknown:
                    await context.PostAsync("Ok.");
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