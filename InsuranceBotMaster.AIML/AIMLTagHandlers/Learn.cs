using System.Xml;
using System.IO;

namespace InsuranceBotMaster.AIML.AIMLTagHandlers
{
    /// <summary>
    /// The learn element instructs the AIML interpreter to retrieve a resource specified by a URI, 
    /// and to process its AIML object contents.
    /// </summary>
    public class Learn : Utils.AIMLTagHandler
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bot">The bot involved in this request</param>
        /// <param name="user">The user making the request</param>
        /// <param name="query">The query that originated this node</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="result">The result to be passed to the user</param>
        /// <param name="templateNode">The node to be processed</param>
        public Learn(AIML.Bot bot,
                        User user,
                        Utils.SubQuery query,
                        Request request,
                        Result result,
                        XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() == "learn")
            {
                // currently only AIML files in the local filesystem can be referenced
                // ToDo: Network HTTP and web service based learning
                if (TemplateNode.InnerText.Length > 0)
                {
                    string path = TemplateNode.InnerText;
                    FileInfo fi = new FileInfo(path);
                    if (fi.Exists)
                    {
                        XmlDocument doc = new XmlDocument();
                        try
                        {
                            doc.Load(path);
                            Bot.LoadAIMLFromXML(doc, path);
                        }
                        catch
                        {
                            Bot.WriteToLog("ERROR! Attempted (but failed) to <learn> some new AIML from the following URI: " + path);
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
