using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Autofac;
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
                logger.LogError("Unhandled exception during dialog.", error.Message, GetException(error));

                try
                {
                    if (Debugger.IsAttached)
                    {
                        var message = _botToUser.MakeMessage();
                        message.Text = $"Exception: { GetException(error)}";
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
                    logger.LogError("Unhandled exception during handling an error.", inner.Message, GetException(inner));
                }

                throw;
            }
        }

        private static string GetException(Exception ex)
        {
            var sb = new StringBuilder();

            if (ex == null) return sb.ToString();

            sb.Append(ex.Message);
            if (ex.InnerException != null)
            {
                sb.Append(GetException(ex.InnerException));
            }
            return sb.ToString();
        }
    }
}