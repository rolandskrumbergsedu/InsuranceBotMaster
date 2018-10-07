using System;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.HybridConversation.Common;
using InsuranceBotMaster.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;

namespace InsuranceBotMaster.Dialogs.HybridConversation.Questions
{
    [Serializable]
    public class WhoCausedDamage : BasicLuisDialog
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(WhoCausedDamage).Name);

            var qnaResult = await QnaHelper.IsQnA(result.Query);

            if (!string.IsNullOrEmpty(qnaResult))
            {
                await context.PostAsync(qnaResult);
                context.Done(true);
            }

            context.Done(false);
        }

        [LuisIntent("Open.DamageCause.OtherVehicle")]
        public async Task OpenDamageCauseOtherVehicleIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(WhoCausedDamage).Name);

            context.Call(new BasicInputTextDialog("Ok. Oppgi registreringsnummeret på kjøretøyet som forårsaket skaden hvis du har dette."), OtherVehicleRegistrationDialogResumeAfter);
        }

        [LuisIntent("Open.DamageCause.OtherDamage")]
        public async Task OpenDamageCauseOtherDamageIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(WhoCausedDamage).Name);

            context.Call(new BasicInputTextDialog("Ok, oppgi skadevolders navn og kontaktinfo (tlf og/eller e-post) hvis du har dette."), OtherVehicleRegistrationDialogResumeAfter);
        }

        [LuisIntent("Open.DamageCause.Unknown")]
        public async Task OpenDamageCauseUnknownIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(WhoCausedDamage).Name);

            await context.PostAsync("Ok.");
            context.Done(false);
        }

        private async Task OtherVehicleRegistrationDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(false);
        }
    }
}