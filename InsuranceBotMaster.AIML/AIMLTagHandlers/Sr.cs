using System.Xml;

namespace InsuranceBotMaster.AIML.AIMLTagHandlers
{
    /// <summary>
    /// The sr element is a shortcut for: 
    /// 
    /// <srai><star/></srai> 
    /// 
    /// The atomic sr does not have any content. 
    /// </summary>
    public class Sr : Utils.AIMLTagHandler
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
        public Sr(AIML.Bot bot,
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
            if (TemplateNode.Name.ToLower() == "sr")
            {
                XmlNode starNode = GetNode("<star/>");
                Star recursiveStar = new Star(Bot, User, Query, Request, Result, starNode);
                string starContent = recursiveStar.Transform();

                XmlNode sraiNode = GetNode("<srai>"+starContent+"</srai>");
                Srai sraiHandler = new Srai(Bot, User, Query, Request, Result, sraiNode);
                return sraiHandler.Transform();
            }
            return string.Empty;
        }
    }
}
