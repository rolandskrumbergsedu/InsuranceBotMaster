using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InsuranceBotMaster.Translation
{
    [Serializable]
    public class Translator
    {
        private readonly string _translatorKey;
        private const string Host = "https://api.microsofttranslator.com";
        private const string Path = "/V2/Http.svc/Translate";

        public Translator(string translatorKey)
        {
            _translatorKey = translatorKey;
        }

        public async Task<string> TranslateFromNorwegianToEnglish(string textToTranslate)
        {
            return await Translate(textToTranslate, "nb-no", "en-us");
        }

        public async Task<string> TranslateFromEnglishToNorwegian(string textToTranslate)
        {
            return await Translate(textToTranslate, "en-us", "nb-no");
        }

        private async Task<string> Translate(string textToTranslate, string culture)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _translatorKey);

                var uri = Host + Path + "?to=" + culture + "&text=" + System.Net.WebUtility.UrlEncode(textToTranslate);

                var response = await client.GetAsync(uri);
                var result = await response.Content.ReadAsStringAsync();

                var content = XElement.Parse(result).Value;

                return content;
            }
        }

        private async Task<string> Translate(string textToTranslate, string fromCulture, string toCulture)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _translatorKey);

                var uri = Host + Path + "?from=" + fromCulture + "&to=" + toCulture + "&text=" + System.Net.WebUtility.UrlEncode(textToTranslate);
                var response = await client.GetAsync(uri);
                var result = await response.Content.ReadAsStringAsync();

                var content = XElement.Parse(result).Value;

                return content;
            }
        }
    }
}
