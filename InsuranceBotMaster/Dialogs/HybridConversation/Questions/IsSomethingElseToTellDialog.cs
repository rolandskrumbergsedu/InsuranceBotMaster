﻿using System;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.HybridConversation.Common;
using InsuranceBotMaster.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;

namespace InsuranceBotMaster.Dialogs.HybridConversation.Questions
{
    [Serializable]
    public class IsSomethingElseToTellDialog : BasicLuisDialog
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(InjuredDialog).Name);

            var qnaResult = await QnaHelper.IsQnA(result.Query);

            if (!string.IsNullOrEmpty(qnaResult))
            {
                await context.PostAsync(qnaResult);
                context.Done(true);
            }

            context.Done(false);
        }

        [LuisIntent("Open.Yes")]
        public async Task YesIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(InjuredDialog).Name);

            context.Call(new BasicInputTextDialog("Ok, hva vil du legge til?"), AdditionalInformationDialogResumeAfter);
        }

        private async Task AdditionalInformationDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(false);
        }

        [LuisIntent("Open.No")]
        public async Task NoIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(InjuredDialog).Name);

            context.Done(false);
        }
    }
}