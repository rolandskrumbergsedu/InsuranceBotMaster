using System;
using System.Configuration;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using InsuranceBotMaster.AIML;
using InsuranceBotMaster.AIML.Utils;
using InsuranceBotMaster.Translation;

namespace InsuranceBotMaster.Helpers
{
    public static class AimlHelper
    {
        public static async Task<string> GetResponseNorwegian(string query)
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

            var utteranceInEnglish = await translator.TranslateFromNorwegianToEnglish(query);

            var userId = Guid.NewGuid().ToString();
            var output = bot.Chat(utteranceInEnglish, userId);

            var resultInNorwegian = await translator.TranslateFromEnglishToNorwegian(output.RawOutput);

            return resultInNorwegian;
        }

        public static string GetResponseEnglish(string query)
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
            var output = bot.Chat(query, userId);
            
            return output.RawOutput;
        }
    }
}