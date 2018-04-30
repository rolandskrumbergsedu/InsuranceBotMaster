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
    public class VehicleDamageDialog : IDialog<object>
    {
        private const string OptionYes = "Ja";
        private const string OptionNo = "Nei";
        private const string OptionDontKnow = "Vet ikke";

        public async Task StartAsync(IDialogContext context)
        {
            var options = new List<string>
            {
                OptionYes,
                OptionNo
            };

            PromptDialog.Choice(
                context: context,
                resume: CarDamagedReceivedAsync,
                options: options,
                prompt: "Ble kjøretøyet skadet?"
            );
        }

        private async Task CarDamagedReceivedAsync(IDialogContext context, IAwaitable<string> result)
        {
            var answer = await result;

            switch (answer)
            {
                case OptionYes:

                    context.Call(new SimpleInputTextDialog("Hvilke skader har kjøretøyet fått?"), VehicleDamageDialogResumeAfter);

                    break;
                case OptionNo:

                    await context.PostAsync("Ok.");
                    context.Done(this);

                    break;
            }
        }

        private async Task VehicleDamageDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new VehicleLocationDialog(), VehicleLocationDialogResumeAfter);
        }

        private async Task VehicleLocationDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Ok.");
            context.Done(this);
        }
    }
}