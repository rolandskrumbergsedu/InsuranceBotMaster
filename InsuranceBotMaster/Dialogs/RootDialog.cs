using System;
using System.Configuration;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using InsuranceBotMaster.AIML;
using InsuranceBotMaster.AIML.Utils;
using InsuranceBotMaster.LUIS;
using InsuranceBotMaster.QnA;
using InsuranceBotMaster.Translation;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace InsuranceBotMaster.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private Translator _translator = new Translator("29569f3847a84fa3ba411afdc2618e1e");

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var activity = await result as Activity;

                var utterance = await TranslateToEnglish(activity?.Text);

                var luisResult = Task.Run(() => QueryLuisIntent(utterance));
                var qnaResult = Task.Run(() => QueryQna(utterance));
                var goalIsSet = GetUserGoal();

                if (goalIsSet)
                {

                }
                else
                {
                    luisResult.Wait();

                    if (luisResult.Result.TopScoringIntent.Score > 0.90)
                    {
                        SaveUserGoal(true);

                        // Load appropriate dialog
                        await HandleLuisMessage(context, luisResult.Result);
                    }
                    else
                    {
                        qnaResult.Wait();
                        if (qnaResult.Result.Answers[0].Score > 0.90)
                        {
                            await HandleQnaMessage(context, qnaResult.Result);
                        }
                        else
                        {
                            await HandleAimlMessage(context, utterance);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                var logger = NLog.LogManager.GetCurrentClassLogger();
                logger.Error($"Exception: {ex.Message}");
                logger.Error($"Stacktrace: {ex.StackTrace}");
            }
            

            context.Wait(MessageReceivedAsync);
        }

        private static bool GetUserGoal()
        {
            return false;
        }

        private static void SaveUserGoal(bool status)
        {

        }

        private async Task<LuisQueryResult> QueryLuisIntent(string utterance)
        {
            var appId = ConfigurationManager.AppSettings["LuisAppId"];
            var luisKey = ConfigurationManager.AppSettings["LuisAPIKey"];

            var caller = new LuisCaller(appId, luisKey);

            var luisResult = await caller.QueryLuis(utterance);

            return luisResult;
        }

        private async Task<QnaQueryResult> QueryQna(string question)
        {
            var kbId = "51a53b34-dc8f-4c2a-899d-2ae926b2d3fb";
            var qnaKey = "d952660440e84528b63bbbb6039b86be";

            var caller = new QnaCaller(kbId, qnaKey);

            var result = await caller.Query(question);

            return result;
        }

        private async Task<string> TranslateToNorwegian(string utteranceInEnglish)
        {
            var result = await _translator.TranslateFromEnglishToNorwegian(utteranceInEnglish);

            return result;
        }

        private async Task<string> TranslateToEnglish(string utteranceInNorwegian)
        {
            var result = await _translator.TranslateFromNorwegianToEnglish(utteranceInNorwegian);

            return result;
        }

        private async Task HandleQnaMessage(IDialogContext context, QnaQueryResult result)
        {

            await context.PostAsync($"QnA score: {result.Answers[0].Score}");
            await context.PostAsync(result.Answers[0].Answer);
        }

        private async Task HandleLuisMessage(IDialogContext context, LuisQueryResult luisResult)
        {
            await context.PostAsync($"Intent: {luisResult.TopScoringIntent.Intent}");
            await context.PostAsync($"Score: {luisResult.TopScoringIntent.Score}");
            await context.PostAsync($"Seems that your car has broken!");
        }

        private async Task HandleAimlMessage(IDialogContext context, string utterance)
        {
            try
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

                var userId = Guid.NewGuid().ToString();
                var output = bot.Chat(utterance, userId);

                var result = await TranslateToNorwegian(output.RawOutput);

                await context.PostAsync(result);
            }
            catch (Exception ex)
            {
                await context.PostAsync("Sorry, something happened. :(");
                await context.PostAsync($"Exception: {ex.Message}");
                await context.PostAsync($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}