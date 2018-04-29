using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.GuidedConversation.ClaimTypes;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation
{
    [Serializable]
    public class ReportClaimDialog : IDialog<object>
    {
        private const string OptionDamageInMotion = "Skade når kjøretøyet var i bevegelse";
        private const string OptionDamageWhenParked = "Skade når kjøretøyet var parkert";
        private const string OptionDamageTechnical = "Teknisk feil eller motorhavari";

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Ok, det skal vi ordne! Vår chatrobot hjelper deg gjennom skademeldingen.");

            var options = new List<string>
            {
                OptionDamageInMotion,
                OptionDamageWhenParked,
                OptionDamageTechnical
            };

            PromptDialog.Choice(
                context: context,
                resume: ChoiceReceivedAsync,
                options: options,
                prompt: "La oss starte med det grunnleggende. Hvilket av disse alternativene passer hendelsen best?"
            );
        }

        public async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var answer = await argument;

            switch (answer)
            {
                case OptionDamageInMotion:
                    context.Call(new DamageInMotionDetailsDialog(), CompleteDialogResumeAfter);
                    break;
                case OptionDamageWhenParked:
                    context.Call(new DamageWhileParkedDetailsDialog(), CompleteDialogResumeAfter);
                    break;
                case OptionDamageTechnical:
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