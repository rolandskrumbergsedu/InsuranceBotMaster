using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using InsuranceBotMaster.Dialogs.GuidedConversation.ClaimTypes;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation
{
    [Serializable]
    public class DamageWhileParkedDetailsDialog : IDialog<object>
    {
        private const string OptionCarTheft = "Tyveri av kjøretøy";
        private const string OptionFireInVehicle = "Brann i kjøretøy";
        private const string OptionBurglaryInCar = "Innbrudd i kjøretøy";
        private const string OptionOtherDamage = "Annen skade";

        public async Task StartAsync(IDialogContext context)
        {
            var options = new List<string>
            {
                OptionCarTheft,
                OptionFireInVehicle,
                OptionBurglaryInCar,
                OptionOtherDamage
            };

            PromptDialog.Choice(
                context: context,
                resume: ChoiceReceivedAsync,
                options: options,
                prompt: "Takk! Hva har skjedd?"
            );
        }

        public async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var answer = await argument;

            switch (answer)
            {
                case OptionCarTheft:
                    await context.PostAsync("Uff da, det var leit å høre.");
                    context.Call(new CarTheftClaimTypeDialog(), CompleteDialogResumeAfter);
                    break;
                case OptionFireInVehicle:
                    await context.PostAsync("Uff da, det var leit å høre.");
                    context.Call(new FireInVehicleClaimTypeDialog(), CompleteDialogResumeAfter);
                    break;
                case OptionBurglaryInCar:
                    await context.PostAsync("Å nei, så kjedelig!");
                    context.Call(new BurglaryInCarClaimTypeDialog(), CompleteDialogResumeAfter);
                    break;
                case OptionOtherDamage:
                    context.Call(new ParkingDamageClaimTypeDialog(), CompleteDialogResumeAfter);
                    break;
            }
        }

        private async Task CompleteDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(this);
        }
    }
}