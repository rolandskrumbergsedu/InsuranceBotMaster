using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using InsuranceBotMaster.Logging;
using Microsoft.Bot.Builder.Dialogs;
using NLog;

namespace InsuranceBotMaster.Dialogs.Technical
{
    [Serializable]
    public class ExceptionHandlerDialog<T> : IExceptionDialog<T>
    {
        private IDialog<T> _dialog;

        private readonly string _message;

        public void SetDialog(IDialog<T> dialog)
        {
            _dialog = dialog;
        }

        public ExceptionHandlerDialog()
        {
        }

        public ExceptionHandlerDialog(string message)
        {
            _message = message;
        }

        public async Task StartAsync(IDialogContext context)
        {
            if (!string.IsNullOrEmpty(_message))
            {
                var logger = LogManager.GetCurrentClassLogger();
                logger.LogError("Exception occured in ExceptionHandler.Message.", _message, string.Empty);

                context.Done("Done");
                return;
            }

            try
            {
                context.Call(_dialog, ResumeAsync);
            }
            catch (Exception e)
            {
                var logger = LogManager.GetCurrentClassLogger();
                logger.LogError("Exception occured in ExceptionHandler.", e.Message, e.StackTrace);
            }
        }

        private static async Task ResumeAsync(IDialogContext context, IAwaitable<T> result)
        {
            try
            {
                context.Done(await result);
            }
            catch (Exception e)
            {
                var logger = LogManager.GetCurrentClassLogger();
                logger.LogError("Exception occured in ResumeAsync.", e.Message, e.StackTrace);
            }
            context.Done("Done");
        }

        private static async Task DisplayExceptionAsync(IDialogContext context, Exception e)
        {

            var stackTrace = e.StackTrace;

            stackTrace = stackTrace.Replace(Environment.NewLine, "  \n");

            var message = e.Message.Replace(Environment.NewLine, "  \n");

            var exceptionStr = $"**{message}**  \n\n{stackTrace}";

            await context.PostAsync($"Debug: {exceptionStr}").ConfigureAwait(false);
        }
    }
}