using System;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using InsuranceBotMaster.AIML;
using InsuranceBotMaster.AIML.Utils;
using InsuranceBotMaster.Dialogs.Common;
using InsuranceBotMaster.Dialogs.HybridConversation.ClaimTypes;
using InsuranceBotMaster.Dialogs.HybridConversation.Common;
using InsuranceBotMaster.QnA;
using InsuranceBotMaster.Translation;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Newtonsoft.Json;

namespace InsuranceBotMaster.Dialogs
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
            var qna = new QnaCaller(ConfigurationManager.AppSettings["QnaAppId"], ConfigurationManager.AppSettings["QnaAppKey"]);

            var qnaResult = await qna.Query(utterance);

            var qnaTopResult = qnaResult.Answers.OrderByDescending(x => x.Score).FirstOrDefault();
            if (qnaTopResult != null && qnaTopResult.Score > 0.8)
            {
                await ShowQnAResult(context, qnaResult, utterance);
                await context.PostAsync(qnaTopResult.Answer);
            }

            // No hits in QnA, lets load up AIML and send translated response
            else
            {
                var cache = MemoryCache.Default;

                if (!(cache["bot"] is Bot bot))
                {
                    var settingsPath = HttpContext.Current.Server.MapPath("~/bin/ConfigurationFiles/Settings.xml");
                    var aimlPath = HttpContext.Current.Server.MapPath("~/bin/AIMLFiles");
                    var basePath = HttpContext.Current.Server.MapPath("~/bin");

                    bot = new Bot(basePath);
                    bot.LoadSettings(settingsPath);
                    var loader = new AIMLLoader(bot);
                    loader.LoadAIML(aimlPath);
                }

                // Calls for AIML should be translated to English
                var translator = new Translator(ConfigurationManager.AppSettings["TranslatorKey"]);

                var utteranceInEnglish = await translator.TranslateFromNorwegianToEnglish(utterance);

                var userId = Guid.NewGuid().ToString();
                var output = bot.Chat(utteranceInEnglish, userId);

                var resultInNorwegian = await translator.TranslateFromEnglishToNorwegian(output.RawOutput);

                await context.PostAsync(resultInNorwegian);
            }
        }

        [LuisIntent("Open.Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hei! Hvordan kan jeg hjelpe deg ?");
        }

        [LuisIntent("Claims.Norway.Motor.ClaimType.Pedestrian")]
        public async Task ClaimsNorwayMotorClaimTypePedestrian(IDialogContext context, LuisResult result)
        {
            context.Call(new CollisionWithPedestrianClaimTypeDialog(), ClaimTypeDialogResumeAfter);
        }

        [LuisIntent("Claims.Norway.Motor.ClaimType.Animal")]
        public async Task ClaimsNorwayMotorClaimTypeAnimal(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry to hear that! Lets report damage on ClaimType.Animal");
        }

        [LuisIntent("Claims.Norway.Motor.ClaimType.Theft")]
        public async Task ClaimsNorwayMotorClaimTypeTheft(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry to hear that! Lets report damage on ClaimType.Theft");
        }

        [LuisIntent("Claims.Norway.Motor.ClaimType.TechnicalFault")]
        public async Task ClaimsNorwayMotorClaimTypeTechnicalFault(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry to hear that! Lets report damage on ClaimType.TechnicalFault");
        }

        [LuisIntent("Claims.Norway.Motor.ClaimType.SingleVehicle")]
        public async Task ClaimsNorwayMotorClaimTypeSingleVehicle(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry to hear that! Lets report damage on ClaimType.SingleVehicle");
        }

        [LuisIntent("Claims.Norway.Motor.ClaimType.Burglary")]
        public async Task ClaimsNorwayMotorClaimTypeBurglary(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry to hear that! Lets report damage on ClaimType.Burglary");
        }

        [LuisIntent("Claims.Norway.Motor.ClaimType.ParkingDamage")]
        public async Task ClaimsNorwayMotorClaimTypeParkingDamage(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry to hear that! Lets report damage on ClaimType.ParkingDamage");
        }

        [LuisIntent("Claims.Norway.Motor.ClaimType.OtherVehicle")]
        public async Task ClaimsNorwayMotorClaimTypeOtherVehicle(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry to hear that! Lets report damage on ClaimType.OtherVehicle");
        }

        [LuisIntent("Claims.Norway.Motor.ClaimType.Fire")]
        public async Task ClaimsNorwayMotorClaimTypeFire(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry to hear that! Lets report damage on ClaimType.Fire");
        }

        [LuisIntent("Claims.Norway.Motor.ClaimType.Cyclist")]
        public async Task ClaimsNorwayMotorClaimTypeCyclist(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry to hear that! Lets report damage on ClaimType.Cyclist");
        }

        [LuisIntent("Claims.Norway.Motor.ReportClaim")]
        public async Task ClaimsNorwayMotorReportClaim(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Kan du beskrive hva som har skjedd?");
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
            await context.PostAsync("Thank you! Is there anything else I can help you with?");
        }

        private async Task CompleteDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Is there anything else I can help you with?");
        }

        private static async Task ShowLuisResult(IDialogContext context, LuisResult result)
        {
            var resultIntents = JsonConvert.SerializeObject(result);
            await context.PostAsync($"You have reached {resultIntents}. You said: {result.Query}");
        }

        private static async Task ShowQnAResult(IDialogContext context, QnaQueryResult answer, string utterance)
        {
            var resultIntents = JsonConvert.SerializeObject(answer);
            await context.PostAsync($"You have reached {resultIntents}. You said: {utterance}");
        }

        private static async Task HandleChildDialogResult(IDialogContext context, IAwaitable<object> result)
        {
            
        }
    }
}