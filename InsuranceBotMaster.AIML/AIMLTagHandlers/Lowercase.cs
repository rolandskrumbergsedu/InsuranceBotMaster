using System.Xml;

namespace InsuranceBotMaster.AIML.AIMLTagHandlers
{
    /// <summary>
    /// The lowercase element tells the AIML interpreter to render the contents of the element 
    /// in lowercase, as defined (if defined) by the locale indicated by the specified language
    /// (if specified). 
    /// 
    /// If no character in this string has a different lowercase version, based on the Unicode 
    /// standard, then the original string is returned. 
    /// </summary>
    public class Lowercase : Utils.AIMLTagHandler
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
        public Lowercase(AIML.Bot bot,
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
            if (TemplateNode.Name.ToLower() == "lowercase")
            {
                return TemplateNode.InnerText.ToLower(Bot.Locale);
            }
            return string.Empty;
        }
    }
}
