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
    public class DamageInMotionDetailsDialog : IDialog<object>
    {
        private const string OptionSingleVehicleAccident = "Eneulykke - kun ett kjøretøy involvert";
        private const string OptionAnimalAccident = "Påkjørsel av dyr";
        private const string OptionMultipleVehicleAccident = "Kollisjon med annet kjøretøy";
        private const string OptionCyclistAccident = "Kollisjon med syklist";
        private const string OptionPedestrianCrossingAccident = "Påkjørsel av fotgjenger";
        private const string OptionFireAccident = "Brann i kjøretøy";
        private const string OptionTechnicalFailureAccident = "Teknisk feil eller motorhavari";

        public async Task StartAsync(IDialogContext context)
        {
            var options = new List<string>
            {
                OptionSingleVehicleAccident,
                OptionAnimalAccident,
                OptionMultipleVehicleAccident,
                OptionCyclistAccident,
                OptionPedestrianCrossingAccident,
                OptionFireAccident,
                OptionTechnicalFailureAccident
            };

            PromptDialog.Choice(
                context: context,
                resume: ChoiceReceivedAsync,
                options: options,
                prompt: "Takk! Og hvilket av disse beskriver best det som har skjedd?"
            );
        }

        public async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var answer = await argument;

            switch (answer)
            {
                case OptionSingleVehicleAccident:
                    await context.PostAsync("Uff da, så kjedelig.");
                    context.Call(new SingleVehicleClaimTypeDialog(), CompleteDialogResumeAfter);
                    break;
                case OptionAnimalAccident:
                    await context.PostAsync("Uff da, det var leit å høre.");
                    context.Call(new CollisionWithAnimalClaimTypeDialog(), CompleteDialogResumeAfter);
                    break;
                case OptionMultipleVehicleAccident:
                    await context.PostAsync("Uff da, det var leit å høre.");
                    context.Call(new CollisionWithAnotherVehicleClaimTypeDialog(), CompleteDialogResumeAfter);
                    break;
                case OptionCyclistAccident:
                    context.Call(new CollisionWithCyclistClaimTypeDialog(), CompleteDialogResumeAfter);
                    break;
                case OptionPedestrianCrossingAccident:
                    context.Call(new CollisionWithPedestrianClaimTypeDialog(), CompleteDialogResumeAfter);
                    break;
                case OptionFireAccident:
                    await context.PostAsync("Uff da, det var leit å høre.");
                    context.Call(new FireInVehicleClaimTypeDialog(), CompleteDialogResumeAfter);
                    break;
                case OptionTechnicalFailureAccident:
                    await context.PostAsync("Uff da, det var leit å høre.");
                    context.Call(new TechnicalFailureClaimTypeDialog(), CompleteDialogResumeAfter);
                    break;
            }
        }

        private async Task CompleteDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(this);
        }
    }
}