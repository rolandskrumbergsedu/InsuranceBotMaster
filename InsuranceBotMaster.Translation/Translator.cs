using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace InsuranceBotMaster.Translation
{
    [Serializable]
    public class Translator
    {
        private readonly string _translatorKey;
        private const string Host = "https://api.cognitive.microsofttranslator.com";
        private const string Path = "/translate?api-version=3.0";

        private const string Norwegian = "nb";
        private const string English = "en";

        public Translator(string translatorKey)
        {
            _translatorKey = translatorKey;
        }

        public async Task<string> TranslateFromNorwegianToEnglish(string textToTranslate)
        {
            var translation = await Translate(textToTranslate, Norwegian, English);
            var result = translation.Translations.FirstOrDefault(x => x.To == English);
            return result != null ? result.Text : string.Empty;
        }

        public async Task<string> TranslateFromEnglishToNorwegian(string textToTranslate)
        {
            var translation = await Translate(textToTranslate, English, Norwegian);
            var result = translation.Translations.FirstOrDefault(x => x.To == Norwegian);
            return result != null ? result.Text : string.Empty;
        }

        public async Task<TranslationResponse> Translate(string textToTranslate, string fromCulture, string toCulture)
        {
            object[] body = { new { Text = textToTranslate } };
            var requestBody = JsonConvert.SerializeObject(body);

            var uri = Host + Path + $"&to={fromCulture}&to={toCulture}";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", _translatorKey);

                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<List<TranslationResponse>>(responseBody);

                return result.FirstOrDefault(x => x.DetectedLanguage.Language == fromCulture);
            }
        }
    }
}
