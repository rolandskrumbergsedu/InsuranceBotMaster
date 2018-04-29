using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using InsuranceBotMaster.Dialogs.GuidedConversation.Common;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation
{
    [Serializable]
    public class BaseStartDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Call(new DateAndRegistrationNumberDialog(), DateAndRegistrationNumberDialogResumeAfter);
        }

        private async Task DateAndRegistrationNumberDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new SimpleInputTextDialog("Hva var kilometerstanden på skadetidspunktet? Skriv det inn i tekstfeltet under. Det holder å gi oss et rundt tall, f.eks. til nærmeste 1000 km."), MilageDialogResumeAfter);
        }

        

        
    }
}