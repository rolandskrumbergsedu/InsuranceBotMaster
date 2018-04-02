using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceBotMaster.LUIS
{
    public class LuisCaller
    {
        private string _appId;
        private string _luisKey;

        public LuisCaller(string appId, string luisKey)
        {
            _appId = appId;
            _luisKey = luisKey;
        }

        public async Task<LuisQueryResult> QueryLuis(string utterance)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://northeurope.api.cognitive.microsoft.com/luis/v2.0/apps/" + _appId);
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _luisKey);

                var content = new StringContent(JsonConvert.SerializeObject(utterance), Encoding.UTF8, "application/json");

                var result = await client.PostAsync("", content);
                string resultContent = await result.Content.ReadAsStringAsync();

                var luisResult = JsonConvert.DeserializeObject<LuisQueryResult>(resultContent);

                return luisResult;
            }
        }
    }

    public class LuisQueryResult
    {
        public string Query { get; set; }
        public TopScoringIntent TopScoringIntent { get; set; }
        public List<Entity> Entities { get; set; }
    }

    public class TopScoringIntent
    {
        public string Intent { get; set; }
        public double Score { get; set; }
    }

    public class Entity
    {

    }
}
