using System;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.GuidedConversation.Common;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation
{
    [Serializable]
    public class IncidentDescriptionDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Kan du forklare hvordan ulykken skjedde?");
            context.Call(new SimpleInputTextDialog("Beskriv gjerne så nøyaktig som mulig - men husk at du vil få mulighet til å gi utfyllende opplysninger når vi tar kontakt med deg hvis du ikke har alle detaljene akkurat nå."), AccidentDescriptionDialogResumeAfter);
        }

        private async Task AccidentDescriptionDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(this);
        }
    }
}