using System;
using System.Diagnostics;
using System.Net.Mime;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using InsuranceBotMaster.Logging;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;
using NLog;

namespace InsuranceBotMaster.Dialogs.Technical
{
    public class PostUnhandledExceptionToUser : IPostToBot
    {
        private readonly IPostToBot _inner;
        private readonly IBotToUser _botToUser;
        private readonly TraceListener _trace;

        public PostUnhandledExceptionToUser(IPostToBot inner, IBotToUser botToUser, ResourceManager resources, TraceListener trace)
        {
            SetField.NotNull(out _inner, nameof(inner), inner);
            SetField.NotNull(out _botToUser, nameof(botToUser), botToUser);
            SetField.NotNull(out _, nameof(resources), resources);
            SetField.NotNull(out _trace, nameof(trace), trace);
        }


        async Task IPostToBot.PostAsync(IActivity activity, CancellationToken token)
        {
            try
            {
                await _inner.PostAsync(activity, token);
            }
            catch (Exception error)
            {
                var logger = LogManager.GetCurrentClassLogger();
                logger.LogError("Unhandled exception during dialog.", error.Message, error.ToString(), activity.Conversation.Id);

                try
                {
                    if (Debugger.IsAttached)
                    {
                        var message = _botToUser.MakeMessage();
                        message.Text = $"Exception: { error }";
                        message.Attachments = new[]
                        {
                            new Attachment(MediaTypeNames.Text.Plain, content: error.StackTrace)
                        };

                        await _botToUser.PostAsync(message, token);
                    }
                    else
                    {
                        var message = _botToUser.MakeMessage();
                        message.Text = "Chatboten har støtt på et uventet problem. Vennligst prøv igjen senere eller ring oss";
                        await _botToUser.PostAsync(message, token);
                    }
                }
                catch (Exception inner)
                {
                    _trace.WriteLine(inner);
                    logger.LogError("Unhandled exception during handling an error.", inner.Message, inner.ToString(), activity.Conversation.Id);
                }

                throw;
            }
        }
    }
}