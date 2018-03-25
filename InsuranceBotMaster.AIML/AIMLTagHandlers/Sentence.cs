using System;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;

namespace InsuranceBotMaster.AIML.AIMLTagHandlers
{
    /// <summary>
    /// The sentence element tells the AIML interpreter to render the contents of the element 
    /// such that the first letter of each sentence is in uppercase, as defined (if defined) by 
    /// the locale indicated by the specified language (if specified). Sentences are interpreted 
    /// as strings whose last character is the period or full-stop character .. If the string does 
    /// not contain a ., then the entire string is treated as a sentence.
    /// 
    /// If no character in this string has a different uppercase version, based on the Unicode 
    /// standard, then the original string is returned. 
    /// </summary>
    public class Sentence : Utils.AIMLTagHandler
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
        public Sentence(AIML.Bot bot,
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
            if(TemplateNode.Name.ToLower()=="sentence")
            {
                if (TemplateNode.InnerText.Length > 0)
                {
                    StringBuilder result = new StringBuilder();
                    char[] letters = TemplateNode.InnerText.Trim().ToCharArray();
                    bool doChange = true;
                    for (int i = 0; i < letters.Length; i++)
                    {
                        string letterAsString = Convert.ToString(letters[i]);
                        if (Bot.Splitters.Contains(letterAsString))
                        {
                            doChange = true;
                        }

                        Regex lowercaseLetter = new Regex("[a-zA-Z]");

                        if (lowercaseLetter.IsMatch(letterAsString))
                        {
                            if (doChange)
                            {
                                result.Append(letterAsString.ToUpper(Bot.Locale));
                                doChange = false;
                            }
                            else
                            {
                                result.Append(letterAsString.ToLower(Bot.Locale));
                            }
                        }
                        else
                        {
                            result.Append(letterAsString);
                        }
                    }
                    return result.ToString();
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
