using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBotMaster.AIML
{
    /// <summary>
    /// Encapsulates all sorts of information about a request to the bot for processing
    /// </summary>
    public class Request
    {
        #region Attributes
        /// <summary>
        /// The raw input from the user
        /// </summary>
        public string RawInput;

        /// <summary>
        /// The time at which this request was created within the system
        /// </summary>
        public DateTime StartedOn;

        /// <summary>
        /// The user who made this request
        /// </summary>
        public User User;

        /// <summary>
        /// The bot to which the request is being made
        /// </summary>
        public Bot Bot;

        /// <summary>
        /// The final result produced by this request
        /// </summary>
        public Result Result;

        /// <summary>
        /// Flag to show that the request has timed out
        /// </summary>
        public bool HasTimedOut = false;

        #endregion

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="rawInput">The raw input from the user</param>
        /// <param name="user">The user who made the request</param>
        /// <param name="bot">The bot to which this is a request</param>
        public Request(string rawInput, User user, Bot bot)
        {
            RawInput = rawInput;
            User = user;
            Bot = bot;
            StartedOn = DateTime.Now;
        }
    }
}
