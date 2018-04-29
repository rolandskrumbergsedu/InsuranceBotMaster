using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace InsuranceBotMaster.Dialogs.GuidedConversation.Common
{
    [Serializable]
    public class SimpleInputTextDialog : IDialog<object>
    {
        private readonly string _question;

        public SimpleInputTextDialog(string questionToAsk)
        {
            _question = questionToAsk;
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(_question);
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            context.Done(this);
        }
    }
}