using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using InsuranceBotMaster.Dialogs.GuidedConversation;
using InsuranceBotMaster.Dialogs.HybridConversation;
using InsuranceBotMaster.Dialogs.Technical;
using InsuranceBotMaster.Logging;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;
using NLog;

namespace InsuranceBotMaster.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                //await Conversation.SendAsync(activity, () => new RootLuisDialog());
                //await Conversation.SendAsync(activity, () => new MainGuidedDialog());

                await Conversation.SendAsync(activity, () => new ExceptionHandlerDialog<object>(new RootLuisDialog()));
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
                //if (message.MembersAdded.Any(o => o.Id == message.Recipient.Id))
                //{
                //    ConnectorClient connector = new ConnectorClient(new System.Uri(message.ServiceUrl));
                //    Activity reply = message.CreateReply("Hi!");
                //    connector.Conversations.ReplyToActivityAsync(reply);
                //    message.Type = ActivityTypes.Message;

                //    reply = message.CreateReply("Velkommen til skadesenteret!");
                //    connector.Conversations.ReplyToActivityAsync(reply);
                //    message.Type = ActivityTypes.Message;
                //}
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
    
}