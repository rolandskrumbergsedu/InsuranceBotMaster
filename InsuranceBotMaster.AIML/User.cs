using System;
using System.Collections.Generic;

namespace InsuranceBotMaster.AIML
{
    /// <summary>
    /// Encapsulates information and history of a user who has interacted with the bot
    /// </summary>
    public class User
    {
        #region Attributes

        /// <summary>
        /// The local instance of the GUID that identifies this user to the bot
        /// </summary>
        private string id;

        /// <summary>
        /// The bot this user is using
        /// </summary>
        public Bot Bot;

        /// <summary>
        /// The GUID that identifies this user to the bot
        /// </summary>
        public string UserID
        {
            get { return Id; }
        }

        /// <summary>
        /// A collection of all the result objects returned to the user in this session
        /// </summary>
        private List<Result> Results = new List<Result>();

        /// <summary>
        /// the value of the "topic" predicate
        /// </summary>
        public string Topic
        {
            get
            {
                return Predicates.GrabSetting("topic");
            }
        }

        /// <summary>
        /// the predicates associated with this particular user
        /// </summary>
        public Utils.SettingsDictionary Predicates;

        /// <summary>
        /// The most recent result to be returned by the bot
        /// </summary>
        public Result LastResult
        {
            get
            {
                if (Results.Count > 0)
                {
                    return Results[0];
                }
                else
                {
                    return null;
                }
            }
        }

        public string Id { get => id; set => id = value; }

        #endregion

        #region Methods

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="UserID">The GUID of the user</param>
        /// <param name="bot">the bot the user is connected to</param>
        public User(string UserID, Bot bot)
        {
            if (UserID.Length > 0)
            {
                Id = UserID;
                Bot = bot;
                Predicates = new Utils.SettingsDictionary(Bot);
                Bot.DefaultPredicates.Clone(Predicates);
                Predicates.AddSetting("topic", "*");
            }
            else
            {
                throw new Exception("The UserID cannot be empty");
            }
        }

        /// <summary>
        /// Returns the string to use for the next that part of a subsequent path
        /// </summary>
        /// <returns>the string to use for that</returns>
        public string getLastBotOutput()
        {
            if (Results.Count > 0)
            {
                return (Results[0]).RawOutput;
            }
            else
            {
                return "*";
            }
        }

        /// <summary>
        /// Returns the first sentence of the last output from the bot
        /// </summary>
        /// <returns>the first sentence of the last output from the bot</returns>
        public string GetThat()
        {
            return GetThat(0, 0);
        }

        /// <summary>
        /// Returns the first sentence of the output "n" steps ago from the bot
        /// </summary>
        /// <param name="n">the number of steps back to go</param>
        /// <returns>the first sentence of the output "n" steps ago from the bot</returns>
        public string GetThat(int n)
        {
            return GetThat(n, 0);
        }

        /// <summary>
        /// Returns the sentence numbered by "sentence" of the output "n" steps ago from the bot
        /// </summary>
        /// <param name="n">the number of steps back to go</param>
        /// <param name="sentence">the sentence number to get</param>
        /// <returns>the sentence numbered by "sentence" of the output "n" steps ago from the bot</returns>
        public string GetThat(int n, int sentence)
        {
            if ((n >= 0) & (n < Results.Count))
            {
                Result historicResult = Results[n];
                if ((sentence >= 0) & (sentence < historicResult.OutputSentences.Count))
                {
                    return historicResult.OutputSentences[sentence];
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns the first sentence of the last output from the bot
        /// </summary>
        /// <returns>the first sentence of the last output from the bot</returns>
        public string GetResultSentence()
        {
            return GetResultSentence(0, 0);
        }

        /// <summary>
        /// Returns the first sentence from the output from the bot "n" steps ago
        /// </summary>
        /// <param name="n">the number of steps back to go</param>
        /// <returns>the first sentence from the output from the bot "n" steps ago</returns>
        public string GetResultSentence(int n)
        {
            return GetResultSentence(n, 0);
        }

        /// <summary>
        /// Returns the identified sentence number from the output from the bot "n" steps ago
        /// </summary>
        /// <param name="n">the number of steps back to go</param>
        /// <param name="sentence">the sentence number to return</param>
        /// <returns>the identified sentence number from the output from the bot "n" steps ago</returns>
        public string GetResultSentence(int n, int sentence)
        {
            if ((n >= 0) & (n < Results.Count))
            {
                Result historicResult = Results[n];
                if ((sentence >= 0) & (sentence < historicResult.InputSentences.Count))
                {
                    return historicResult.InputSentences[sentence];
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Adds the latest result from the bot to the Results collection
        /// </summary>
        /// <param name="latestResult">the latest result from the bot</param>
        public void AddResult(Result latestResult)
        {
            Results.Insert(0, latestResult);
        }
        #endregion
    }
}