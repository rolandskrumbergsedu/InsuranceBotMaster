using System;
using System.Text;
using System.Collections.Generic;

namespace InsuranceBotMaster.AIML.Normalize
{
    /// <summary>
    /// Splits the raw input into its constituent sentences. Split using the tokens found in 
    /// the bots Splitters string array.
    /// </summary>
    public class SplitIntoSentences
    {
        /// <summary>
        /// The bot this sentence splitter is associated with
        /// </summary>
        private Bot Bot;

        /// <summary>
        /// The raw input string
        /// </summary>
        private string _inputString;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bot">The bot this sentence splitter is associated with</param>
        /// <param name="inputString">The raw input string to be processed</param>
        public SplitIntoSentences(Bot bot, string inputString)
        {
            Bot = bot;
            _inputString = inputString;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bot">The bot this sentence splitter is associated with</param>
        public SplitIntoSentences(Bot bot)
        {
            Bot = bot;
        }

        /// <summary>
        /// Splits the supplied raw input into an array of strings according to the tokens found in
        /// the bot's Splitters List<>
        /// </summary>
        /// <param name="inputString">The raw input to split</param>
        /// <returns>An array of strings representing the constituent "sentences"</returns>
        public string[] Transform(string inputString)
        {
            _inputString = inputString;
            return Transform();
        }

        /// <summary>
        /// Splits the raw input supplied via the ctor into an array of strings according to the tokens
        /// found in the bot's Splitters List<>
        /// </summary>
        /// <returns>An array of strings representing the constituent "sentences"</returns>
        public string[] Transform()
        {
            string[] tokens = Bot.Splitters.ToArray();
            string[] rawResult = _inputString.Split(tokens, StringSplitOptions.RemoveEmptyEntries);
            List<string> tidyResult = new List<string>();
            foreach (string rawSentence in rawResult)
            {
                string tidySentence = rawSentence.Trim();
                if (tidySentence.Length > 0)
                {
                    tidyResult.Add(tidySentence);
                }
            }
            return tidyResult.ToArray();
        }
    }
}
