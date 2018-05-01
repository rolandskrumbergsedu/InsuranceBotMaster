using System;
using System.Threading.Tasks;
using InsuranceBotMaster.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace InsuranceBotMaster.Dialogs
{
    [Serializable]
    public class BasicInputTextDialog : IDialog<object>
    {
        private readonly string _question;

        public BasicInputTextDialog(string questionToAsk)
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
            var answer = await QnaHelper.IsQnA(argument);

            if (!string.IsNullOrEmpty(answer))
            {
                await context.PostAsync(answer);
                context.Done(true);
            }
            else
            {
                context.Done(false);
            }
        }
    }
}