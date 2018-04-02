using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceBotMaster.QnA
{
    public class QnaCaller
    {
        private string _kbId;
        private string _key;

        public QnaCaller(string kbId, string key)
        {
            _kbId = kbId;
            _key = key;
        }

        public async Task<QnaQueryResult> Query(string question)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://westus.api.cognitive.microsoft.com/qnamaker/v2.0/knowledgebases/" + _kbId + "/");
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _key);

                var query = new QnaQuery(question);

                var content = new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json");

                var result = await client.PostAsync("generateAnswer", content);
                string resultContent = await result.Content.ReadAsStringAsync();

                var luisResult = JsonConvert.DeserializeObject<QnaQueryResult>(resultContent);

                return luisResult;
            }
        }
    }

    public class QnaQuery
    {
        public QnaQuery(string question)
        {
            Question = question;
        }

        public string Question { get; set; }
    }

    public class QnaQueryResult
    {
        public List<QnaAnswer> Answers { get; set; }
    }

    public class QnaAnswer
    {
        public string Answer { get; set; }
        public List<string> Questions { get; set; }
        public double Score { get; set; }
    }
}
