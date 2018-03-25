using System.Xml;
using System.Collections.Generic;

namespace InsuranceBotMaster.AIML.AIMLTagHandlers
{
    /// <summary>
    /// The random element instructs the AIML interpreter to return exactly one of its contained li 
    /// elements randomly. The random element must contain one or more li elements of type 
    /// defaultListItem, and cannot contain any other elements.
    /// </summary>
    public class Random : Utils.AIMLTagHandler
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
        public Random(AIML.Bot bot,
                        User user,
                        Utils.SubQuery query,
                        Request request,
                        Result result,
                        XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
            IsRecursive = false;
        }

        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() == "random")
            {
                if (TemplateNode.HasChildNodes)
                {
                    // only grab <li> nodes
                    List<XmlNode> listNodes = new List<XmlNode>();
                    foreach (XmlNode childNode in TemplateNode.ChildNodes)
                    {
                        if (childNode.Name == "li")
                        {
                            listNodes.Add(childNode);
                        }
                    }
                    if (listNodes.Count > 0)
                    {
                        global::System.Random r = new global::System.Random();
                        XmlNode chosenNode = listNodes[r.Next(listNodes.Count)];
                        return chosenNode.InnerXml;
                    }
                }
            }
            return string.Empty;
        }
    }
}
