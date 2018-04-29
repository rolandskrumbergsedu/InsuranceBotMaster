﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;

namespace InsuranceBotMaster.Dialogs.GuidedConversation.ClaimTypes
{
    [Serializable]
    public class CarTheftClaimTypeDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("This is Car Theft Claim Type");
            context.Done(this);
        }
    }
}