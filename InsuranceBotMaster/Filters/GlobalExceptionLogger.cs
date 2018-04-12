using System.Web.Http.ExceptionHandling;

namespace InsuranceBotMaster.Filters
{
    public class GlobalExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Error($"GlobalExceptionLogger: {context.Exception.Message}");
            logger.Error($"GlobalExceptionLogger: {context.Exception.StackTrace}");
            
        }
    }
}