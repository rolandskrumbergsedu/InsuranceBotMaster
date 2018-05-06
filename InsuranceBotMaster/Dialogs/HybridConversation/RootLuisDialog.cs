using System;
using System.Threading.Tasks;
using InsuranceBotMaster.Dialogs.Common;
using InsuranceBotMaster.Dialogs.HybridConversation.ClaimTypes;
using InsuranceBotMaster.Dialogs.HybridConversation.Common;
using InsuranceBotMaster.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;

namespace InsuranceBotMaster.Dialogs.HybridConversation
{
    [Serializable]
    public class RootLuisDialog : BasicLuisDialog
    {


        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
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
            await context.PostAsync("Hei! Hvordan kan jeg hjelpe deg?");
        }

        [LuisIntent("Claims.Norway.Motor.ClaimType.Pedestrian")]
        public async Task ClaimsNorwayMotorClaimTypePedestrian(IDialogContext context, LuisResult result)
        {
            context.Call(new CollisionWithPedestrianClaimTypeDialog(), ClaimTypeDialogResumeAfter);
        }

        [LuisIntent("Claims.Norway.Motor.ClaimType.Animal")]
        public async Task ClaimsNorwayMotorClaimTypeAnimal(IDialogContext context, LuisResult result)
        {
            context.Call(new CollisionWithAnimalClaimTypeDialog(), ClaimTypeDialogResumeAfter);
        }

        [LuisIntent("Claims.Norway.Motor.ClaimType.Theft")]
        public async Task ClaimsNorwayMotorClaimTypeTheft(IDialogContext context, LuisResult result)
        {
            context.Call(new CarTheftClaimTypeDialog(), ClaimTypeDialogResumeAfter);
        }

        [LuisIntent("Claims.Norway.Motor.ClaimType.TechnicalFault")]
        public async Task ClaimsNorwayMotorClaimTypeTechnicalFault(IDialogContext context, LuisResult result)
        {
            context.Call(new TechnicalFailureClaimTypeDialog(), ClaimTypeDialogResumeAfter);
        }

        [LuisIntent("Claims.Norway.Motor.ClaimType.SingleVehicle")]
        public async Task ClaimsNorwayMotorClaimTypeSingleVehicle(IDialogContext context, LuisResult result)
        {
            context.Call(new SingleVehicleClaimTypeDialog(), ClaimTypeDialogResumeAfter);
        }

        [LuisIntent("Claims.Norway.Motor.ClaimType.Burglary")]
        public async Task ClaimsNorwayMotorClaimTypeBurglary(IDialogContext context, LuisResult result)
        {
            context.Call(new BurglaryInCarClaimTypeDialog(), ClaimTypeDialogResumeAfter);
        }

        [LuisIntent("Claims.Norway.Motor.ClaimType.ParkingDamage")]
        public async Task ClaimsNorwayMotorClaimTypeParkingDamage(IDialogContext context, LuisResult result)
        {
            context.Call(new ParkingDamageClaimTypeDialog(), ClaimTypeDialogResumeAfter);
        }

        [LuisIntent("Claims.Norway.Motor.ClaimType.OtherVehicle")]
        public async Task ClaimsNorwayMotorClaimTypeOtherVehicle(IDialogContext context, LuisResult result)
        {
            context.Call(new CollisionWithAnotherVehicleClaimTypeDialog(), ClaimTypeDialogResumeAfter);
        }

        [LuisIntent("Claims.Norway.Motor.ClaimType.Fire")]
        public async Task ClaimsNorwayMotorClaimTypeFire(IDialogContext context, LuisResult result)
        {
            context.Call(new FireInVehicleClaimTypeDialog(), ClaimTypeDialogResumeAfter);
        }

        [LuisIntent("Claims.Norway.Motor.ClaimType.Cyclist")]
        public async Task ClaimsNorwayMotorClaimTypeCyclist(IDialogContext context, LuisResult result)
        {
            context.Call(new CollisionWithCyclistClaimTypeDialog(), ClaimTypeDialogResumeAfter);
        }

        [LuisIntent("Claims.Norway.Motor.ReportClaim")]
        public async Task ClaimsNorwayMotorReportClaim(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Ok. Can you explain shortly what has happened?");
        }

        [LuisIntent("Claims.Norway.Motor.RoadAssistance")]
        public async Task ClaimsNorwayMotorRoadAssistance(IDialogContext context, LuisResult result)
        {
            context.Call(new RoadAssistanceDialog(), CompleteDialogResumeAfter);
        }

        [LuisIntent("Claims.Norway.Motor.AutoGlass")]
        public async Task ClaimsNorwayMotorAutoGlass(IDialogContext context, LuisResult result)
        {
            context.Call(new AutoGlassDialog(), CompleteDialogResumeAfter);
        }

        private async Task ClaimTypeDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("If there is something else I can help you with I will be glad to assist you?");
        }

        private async Task CompleteDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("If there is something else I can help you with I will be glad to assist you?");
        }
    }
}