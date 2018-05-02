using System;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.HybridConversation.Common;
using InsuranceBotMaster.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;

namespace InsuranceBotMaster.Dialogs.HybridConversation.Questions
{
    [Serializable]
    public class WhereIsCarNowDialog : BasicLuisDialog
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

        [LuisIntent("Open.Place.DailyUse")]
        public async Task OpenDailyUseIntent(IDialogContext context, LuisResult result)
        {
            context.Done(false);
        }

        [LuisIntent("Open.Place.Home")]
        public async Task OpenPlaceHomeIntent(IDialogContext context, LuisResult result)
        {
            context.Done(false);
        }

        [LuisIntent("Open.Place.Workshop")]
        public async Task OpenPlaceWorkshopIntent(IDialogContext context, LuisResult result)
        {
            context.Call(new BasicInputTextDialog("Hvilket verksted?"), WorkshopDialogResumeAfter);
        }

        private async Task WorkshopDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(false);
        }

        [LuisIntent("Open.Place.Other")]
        public async Task OpenPlaceOtherIntent(IDialogContext context, LuisResult result)
        {
            context.Call(new BasicInputTextDialog("Hvor da?"), SomewhereElseDialogResumeAfter);
        }

        private async Task SomewhereElseDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(false);
        }

        [LuisIntent("Open.DontKnow")]
        public async Task OpenDontKnowIntent(IDialogContext context, LuisResult result)
        {
            context.Done(false);
        }
    }
}