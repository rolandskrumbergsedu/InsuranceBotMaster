using System.Xml;

namespace InsuranceBotMaster.AIML.AIMLTagHandlers
{
    /// <summary>
    /// The atomic version of the gender element is a shortcut for:
    /// 
    /// <gender><star/></gender> 
    ///
    /// The atomic gender does not have any content. 
    /// 
    /// The non-atomic gender element instructs the AIML interpreter to: 
    /// 
    /// 1. replace male-gendered words in the result of processing the contents of the gender element 
    /// with the grammatically-corresponding female-gendered words; and 
    /// 
    /// 2. replace female-gendered words in the result of processing the contents of the gender element 
    /// with the grammatically-corresponding male-gendered words. 
    /// 
    /// The definition of "grammatically-corresponding" is left up to the implementation.
    /// 
    /// Historically, implementations of gender have exclusively dealt with pronouns, likely due to the 
    /// fact that most AIML has been written in English. However, the decision about whether to 
    /// transform gender of other words is left up to the implementation.
    /// </summary>
    public class Gender : Utils.AIMLTagHandler
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
        public Gender(AIML.Bot bot,
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
            if (TemplateNode.Name.ToLower() == "gender")
            {
                if (TemplateNode.InnerText.Length > 0)
                {
                    // non atomic version of the node
                    return Normalize.ApplySubstitutions.Substitute(Bot, Bot.GenderSubstitutions, TemplateNode.InnerText);
                }
                else
                {
                    // atomic version of the node
                    XmlNode starNode = GetNode("<star/>");
                    Star recursiveStar = new Star(Bot, User, Query, Request, Result, starNode);
                    TemplateNode.InnerText = recursiveStar.Transform();
                    if (TemplateNode.InnerText.Length > 0)
                    {
                        return ProcessChange();
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
            return string.Empty;
        }
    }
}
