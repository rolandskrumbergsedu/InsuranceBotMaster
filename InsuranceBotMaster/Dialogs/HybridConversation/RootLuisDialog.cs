﻿using System;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.Common;
using InsuranceBotMaster.Dialogs.HybridConversation.ClaimTypes;
using InsuranceBotMaster.Dialogs.HybridConversation.Common;
using InsuranceBotMaster.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using NLog;

namespace InsuranceBotMaster.Dialogs.HybridConversation
{
    [Serializable]
    public class RootLuisDialog : BasicLuisDialog
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(RootLuisDialog).Name);

            var utterance = result.Query;

            // If not, check against QnA, if there is a hit, use it, if no, use AIML response
            var qnaResult = await QnaHelper.IsQnA(utterance);

            if (!string.IsNullOrEmpty(qnaResult))
            {
                await context.PostAsync(qnaResult);
            }
            else
            {
                await context.PostAsync(await AimlHelper.GetResponseNorwegian(utterance));
            }
        }

        [LuisIntent("Open.Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(RootLuisDialog).Name);

            await context.PostAsync("Hei! Hvordan kan jeg hjelpe deg?");
        }

        [LuisIntent("ClaimType.CollisionWithPedestrian")]
        public async Task ClaimsNorwayMotorClaimTypePedestrian(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(RootLuisDialog).Name);

            context.Call(new CollisionWithPedestrianClaimTypeDialog(), ClaimTypeDialogResumeAfter);
        }

        [LuisIntent("ClaimType.CollisionWithAnimal")]
        public async Task ClaimsNorwayMotorClaimTypeAnimal(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(RootLuisDialog).Name);

            context.Call(new CollisionWithAnimalClaimTypeDialog(), ClaimTypeDialogResumeAfter);
        }

        [LuisIntent("ClaimType.Theft")]
        public async Task ClaimsNorwayMotorClaimTypeTheft(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(RootLuisDialog).Name);

            context.Call(new CarTheftClaimTypeDialog(), ClaimTypeDialogResumeAfter);
        }

        [LuisIntent("ClaimType.TechnicalFault")]
        public async Task ClaimsNorwayMotorClaimTypeTechnicalFault(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(RootLuisDialog).Name);

            context.Call(new TechnicalFailureClaimTypeDialog(), ClaimTypeDialogResumeAfter);
        }

        [LuisIntent("ClaimType.SingleVehicleAccident")]
        public async Task ClaimsNorwayMotorClaimTypeSingleVehicle(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(RootLuisDialog).Name);

            context.Call(new SingleVehicleClaimTypeDialog(), ClaimTypeDialogResumeAfter);
        }

        [LuisIntent("ClaimType.Burglary")]
        public async Task ClaimsNorwayMotorClaimTypeBurglary(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(RootLuisDialog).Name);

            context.Call(new BurglaryInCarClaimTypeDialog(), ClaimTypeDialogResumeAfter);
        }

        [LuisIntent("ClaimType.ParkingDamage")]
        public async Task ClaimsNorwayMotorClaimTypeParkingDamage(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(RootLuisDialog).Name);

            context.Call(new ParkingDamageClaimTypeDialog(), ClaimTypeDialogResumeAfter);
        }

        [LuisIntent("ClaimType.CollisionWithAnotherVehicle")]
        public async Task ClaimsNorwayMotorClaimTypeOtherVehicle(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(RootLuisDialog).Name);

            context.Call(new CollisionWithAnotherVehicleClaimTypeDialog(), ClaimTypeDialogResumeAfter);
        }

        [LuisIntent("ClaimType.Fire")]
        public async Task ClaimsNorwayMotorClaimTypeFire(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(RootLuisDialog).Name);

            context.Call(new FireInVehicleClaimTypeDialog(), ClaimTypeDialogResumeAfter);
        }

        [LuisIntent("ClaimType.CollisionWithCyclist")]
        public async Task ClaimsNorwayMotorClaimTypeCyclist(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(RootLuisDialog).Name);

            context.Call(new CollisionWithCyclistClaimTypeDialog(), ClaimTypeDialogResumeAfter);
        }

        [LuisIntent("ReportClaim")]
        public async Task ClaimsNorwayMotorReportClaim(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(RootLuisDialog).Name);

            await context.PostAsync("Ok. Kan du forklare hva som skjedde?");
        }

        [LuisIntent("RoadAssistance")]
        public async Task ClaimsNorwayMotorRoadAssistance(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(RootLuisDialog).Name);

            context.Call(new RoadAssistanceDialog(), CompleteDialogResumeAfter);
        }

        [LuisIntent("AutoGlass")]
        public async Task ClaimsNorwayMotorAutoGlass(IDialogContext context, LuisResult result)
        {
            LogHelper.LogLuisResult(result, context.Activity, typeof(RootLuisDialog).Name);

            context.Call(new AutoGlassDialog(), CompleteDialogResumeAfter);
        }

        private async Task ClaimTypeDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Om det er noe annet jeg kan hjelpe deg med, vil jeg gjerne gjøre det.");
        }

        private async Task CompleteDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Om det er noe annet jeg kan hjelpe deg med, vil jeg gjerne gjøre det.");
        }
    }
}