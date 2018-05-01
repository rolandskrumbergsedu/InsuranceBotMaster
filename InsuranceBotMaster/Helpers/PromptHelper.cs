using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Helpers
{
    public static class PromptHelper
    {
        public static PromptOptions<string> CreatePromptOptions(List<string> options, string prompt)
        {
            return new PromptOptions<string>(
                prompt: prompt,
                retry: "Sorry, I did not understand you!",
                tooManyAttempts: "I do not get you.",
                options: options,
                attempts: 2);
        }

        public static async Task HandleTooManyAttempts(IDialogContext context)
        {
            await context.PostAsync("It seems that I am not helping you.");
            await context.PostAsync("I will start over.");
            context.EndConversation("TooManyAttempts");
        }
    }
}