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
                retry: "Beklager, jeg forstod deg ikke!",
                tooManyAttempts: "Jeg skjønner deg ikke.",
                options: options,
                attempts: 2);
        }

        public static async Task HandleTooManyAttempts(IDialogContext context)
        {
            await context.PostAsync("Det ser ut til at jeg ikke hjelper deg.");
            await context.PostAsync("Jeg skal begynne igjen.");
            context.EndConversation("TooManyAttempts");
        }
    }
}