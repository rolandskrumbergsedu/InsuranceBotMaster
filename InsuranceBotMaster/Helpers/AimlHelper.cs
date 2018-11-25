using System;
using System.Configuration;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using InsuranceBotMaster.AIML;
using InsuranceBotMaster.AIML.Utils;
using InsuranceBotMaster.Logging;
using InsuranceBotMaster.Translation;

namespace InsuranceBotMaster.Helpers
{
    public static class AimlHelper
    {
        public static async Task<string> GetResponseNorwegian(string query)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();

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
            logger.LogTranslationResult(query, utteranceInEnglish, "Norwegian", "English");

            var userId = Guid.NewGuid().ToString();
            var output = bot.Chat(utteranceInEnglish, userId);
            logger.LogAimlResult(utteranceInEnglish, output.RawOutput);

            var resultInNorwegian = await translator.TranslateFromEnglishToNorwegian(output.RawOutput);

            resultInNorwegian = FirstLetterToUpper(resultInNorwegian);

            logger.LogTranslationResult(output.RawOutput, resultInNorwegian, "English", "Norwegian");

            return resultInNorwegian;
        }

        public static string GetResponseEnglish(string query)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();

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
            logger.LogAimlResult(query, output.RawOutput);

            return output.RawOutput;
        }

        public static string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }
    }
}