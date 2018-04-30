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
    public class InjuredFireDialog : IDialog<object>
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
                prompt: "Ble noen skadet i brannen?"
            );
        }

        public async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var answer = await argument;

            switch (answer)
            {
                case OptionYes:
                    await context.PostAsync("Håper skadene ikke er alvorlige.");
                    await context.PostAsync("Siden det har oppstått en personskade trenger vi litt info om den eller de som har blitt skadet, slik at vi kan følge opp dette.");
                    context.Call(new InjuredDialog(), InjuredDialogResumeAfter);
                    break;
                case OptionNo:
                    await context.PostAsync("Det var godt å høre.");
                    context.Done(this);
                    break;
            }
        }

        private async Task InjuredDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(this);
        }
    }
}