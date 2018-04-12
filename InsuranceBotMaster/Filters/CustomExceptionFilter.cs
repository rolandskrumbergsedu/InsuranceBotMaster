using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace InsuranceBotMaster.Filters
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Error($"ExceptionFilter: {actionExecutedContext.Exception.Message}");
            logger.Error($"ExceptionFilter: {actionExecutedContext.Exception.StackTrace}");

            if (actionExecutedContext.Exception.InnerException != null)
            {
                logger.Error($"ExceptionFilter: {actionExecutedContext.Exception.InnerException.Message}");
                logger.Error($"ExceptionFilter: {actionExecutedContext.Exception.InnerException.StackTrace}");
            }
            //We can log this exception message to the file or database.  
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An unhandled exception was thrown by service."),  
                ReasonPhrase = "Internal Server Error.Please Contact your Administrator."
            };
            actionExecutedContext.Response = response;
        }
    }
}