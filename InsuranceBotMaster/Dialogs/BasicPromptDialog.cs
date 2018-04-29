using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using InsuranceBotMaster.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace InsuranceBotMaster.Dialogs
{
    [Serializable]
    public class BasicPromptDialog : IDialog<object>
    {
        public IEnumerable<string> _options = new List<string>()
        {
            "a",
            "b"
        };

        public async Task StartAsync(IDialogContext context)
        {
            PromptDialog.Choice(
                context: context,
                resume: MessageReceivedAsync,
                options: _options,
                prompt: "What best describes your situation?",
                retry: "Please, try again."
            );
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var answer = await argument;

            // Based on the choice, do next action
            await context.PostAsync("MessageReceivedAsync: " + answer);
            context.Done(this);
        }

        //public virtual async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<string> activity)
        //{
        //    var answer = await activity;

        //    // Based on the choice, do next action
        //    await context.PostAsync("ChoiceReceivedAsync: " + answer);
        //    context.Done(this);
        //}
    }
}