﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using InsuranceBotMaster.Dialogs.HybridConversation.Common;
using InsuranceBotMaster.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;

namespace InsuranceBotMaster.Dialogs.HybridConversation.Questions
{
    [Serializable]
    public class DamagedItemOwnerDialog : BasicLuisDialog
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            var qnaResult = await QnaHelper.IsQnA(result.Query);

            if (!string.IsNullOrEmpty(qnaResult))
            {
                await context.PostAsync(qnaResult);
                context.Done(true);
            }

            context.Done(false);
        }

        [LuisIntent("Open.Myself")]
        public async Task YesIntent(IDialogContext context, LuisResult result)
        {
            context.Done(false);
        }

        [LuisIntent("Open.Other")]
        public async Task NoIntent(IDialogContext context, LuisResult result)
        {
            context.Call(new BasicInputTextDialog("Hva er kontaktinformasjonen til eieren (f.eks. navn, e-post, telefon)?"), OwnerInformationDialogResumeAfter);
        }

        private async Task OwnerInformationDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var qnaInvoked = Convert.ToBoolean(await result);

            if (!qnaInvoked)
            {
                context.Done(false);
            }
            else
            {
                context.Call(new BasicInputTextDialog("Hva er kontaktinformasjonen til eieren (f.eks. navn, e-post, telefon)?"), OwnerInformationDialogResumeAfter);
            }
        }
    }
}