using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InsuranceBotMaster.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using InsuranceBotMaster.Dialogs.Common;

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
            context.Wait(MessageReceivedAsync);
        }

        public async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            try
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
            catch (TooManyAttemptsException)
            {
                await PromptHelper.HandleTooManyAttempts(context);
            }
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var activity = await argument;

            if (activity.Type != ActivityTypes.Message)
            {
                return;
            }

            var options = new List<string>
            {
                OptionHelp,
                OptionGlass,
                OptionClaim
            };

            var prompt = PromptHelper.CreatePromptOptions(options, "Hva kan vi hjelpe deg med?");
            PromptDialog.Choice(context, ChoiceReceivedAsync, prompt);
        }

        private async Task CompleteDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Om det er noe annet jeg kan hjelpe deg med, vil jeg gjerne gjøre det.");
        }
    }
}