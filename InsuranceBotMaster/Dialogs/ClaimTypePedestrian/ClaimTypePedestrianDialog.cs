using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace InsuranceBotMaster.Dialogs.ClaimTypePedestrian
{
    [Serializable]
    public class ClaimTypePedestrianDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Lets report a claim for pedestrian claim type.");
            context.Wait(this.MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            context.Call(new BasicInputTextDialog("When the incident happened?"), IncidentDateDialogResumeAfter);
            //var message = await argument;
            //context.Forward(new BasicInputTextDialog("When the incident happened?"), IncidentDateDialogResumeAfter, message, CancellationToken.None);
        }

        private async Task IncidentDateDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var resultAwaited = await result;
            var resultFromChild = Convert.ToBoolean(resultAwaited);

            if (!resultFromChild)
            {
                context.Call(new BasicInputTextDialog("Describe in details what happened."), WhatHappenedDialogDialogResumeAfter);
            }
            else
            {
                //QnA was invoked, we need to call again
                context.Call(new BasicInputTextDialog("When the incident happened?"), IncidentDateDialogResumeAfter);
            }
        }

        private async Task WhatHappenedDialogDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var resultAwaited = await result;
            var resultFromChild = Convert.ToBoolean(resultAwaited);

            if (!resultFromChild)
            {
                context.Call(new BasicInputTextDialog("What is your email address?"), ContactInfoDialogDialogResumeAfter);
            }
            else
            {
                //QnA was invoked, we need to call again
                context.Call(new BasicInputTextDialog("Describe in details what happened."), WhatHappenedDialogDialogResumeAfter);
            }
        }

        private async Task ContactInfoDialogDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var resultFromChild = Convert.ToBoolean(await result);

            if (!resultFromChild)
            {
                await context.PostAsync("Thank you! We will review this and give you a call! :)");
                await context.PostAsync("If there is something else I cando for you, I would be glad to help.");
                context.Done(result);
            }
            else
            {
                //QnA was invoked, we need to call again
                context.Call(new BasicInputTextDialog("What is your email address?"), ContactInfoDialogDialogResumeAfter);
            }

            
        }
    }
}