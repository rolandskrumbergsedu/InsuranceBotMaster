using System;
using System.Threading.Tasks;
using System.Web;
using InsuranceBotMaster.AIML;
using InsuranceBotMaster.AIML.Utils;
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
            
            context.Wait(MessageReceivedAsync);
        }
    }
}