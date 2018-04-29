using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation
{
    [Serializable]
    public class InjuredPersonDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Done(this);
        }
    }
}