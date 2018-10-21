using System;
using System.Threading.Tasks;
using InsuranceBotMaster.Logging;
using Microsoft.Bot.Builder.Dialogs;
using NLog;

namespace InsuranceBotMaster.Dialogs.Technical
{
    [Serializable]
    public class ExceptionHandlerDialog<T> : IExceptionDialog<T>
    {
        private IDialog<T> _dialog;
        public void SetDialog(IDialog<T> dialog)
        {
            _dialog = dialog;
        }

        public ExceptionHandlerDialog()
        {
        }

        public ExceptionHandlerDialog(IDialog<T> dialog)
        {
            _dialog = dialog;
        }

        public async Task StartAsync(IDialogContext context)
        {
            try
            {
                context.Call(_dialog, ResumeAsync);
            }
            catch (Exception e)
            {
                var logger = LogManager.GetCurrentClassLogger();
                logger.LogError("Exception occured in ExceptionHandler.", e.Message, e.StackTrace, context.Activity.Conversation.Id);
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
                logger.LogError("Exception occured in ResumeAsync.", e.Message, e.ToString(), context.Activity.Conversation.Id);
            }
            context.Done("Done");
        }
    }
}