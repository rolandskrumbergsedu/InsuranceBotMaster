using System;
using System.Threading.Tasks;
using System.Web;
using InsuranceBotMaster.AIML;
using InsuranceBotMaster.AIML.Utils;
using InsuranceBotMaster.LUIS;
using InsuranceBotMaster.QnA;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace InsuranceBotMaster.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            var luisResult = await QueryLuisIntent(activity);
            var qnaResult = await QueryQna(activity);

            if (luisResult.TopScoringIntent.Score > 0.95)
            {
                await HandleLuisMessage(context, luisResult);
            }
            else if (qnaResult.Answers[0].Score > 0.95)
            {
                await HandleQnaMessage(context, qnaResult);
            }
            else
            {
                await HandleAimlMessage(context, activity);
            }
            
            context.Wait(MessageReceivedAsync);
        }

        private async Task<LuisQueryResult> QueryLuisIntent(Activity activity)
        {
            var appId = "c3b286b3-832e-426b-b9e2-6d849d5a28c6";
            var luisKey = "92c76cdbf18c4d1db7c2e8e995e29ee2";

            var caller = new LuisCaller(appId, luisKey);

            var utterance = activity.Text;

            var luisResult = await caller.QueryLuis(utterance);

            return luisResult;
        }

        private async Task<QnaQueryResult> QueryQna(Activity activity)
        {
            var kbId = "51a53b34-dc8f-4c2a-899d-2ae926b2d3fb";
            var qnaKey = "d952660440e84528b63bbbb6039b86be";

            var caller = new QnaCaller(kbId, qnaKey);

            var question = activity.Text;

            var result = await caller.Query(question);

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

        private async Task HandleAimlMessage(IDialogContext context, Activity activity)
        {
            try
            {
                var settingsPath = HttpContext.Current.Server.MapPath("~/bin/ConfigurationFiles/Settings.xml");
                var aimlPath = HttpContext.Current.Server.MapPath("~/bin/AIMLFiles");
                var basePath = HttpContext.Current.Server.MapPath("~/bin");

                var bot = new Bot(basePath);
                bot.LoadSettings(settingsPath);
                var loader = new AIMLLoader(bot);
                loader.LoadAIML(aimlPath);
                var userId = Guid.NewGuid().ToString();
                var output = bot.Chat(activity.Text, userId);
                await context.PostAsync(output.RawOutput);
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