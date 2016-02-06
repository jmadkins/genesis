﻿using System;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Filters;
using eSIS.Core.Exceptions;
using NLog;

namespace eSIS.Core.API.Exceptions
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is ApiErrorExcpetion)
            {
                var exception = (ApiErrorExcpetion)context.Exception;
                HandleError(context, LogLevel.Fatal, exception.OverridenHttpStatusCode ?? HttpStatusCode.ServiceUnavailable);
            }
            else if (context.Exception is ValidationException)
            {

            }
            else if (context.Exception is HttpResponseException)
            {
                var exception = (HttpResponseException) context.Exception;
                Log(LogLevel.Debug, exception);
                context.Response = exception.Response;
            }
            else if (context.Exception is RequiredHeaderException)
            {
                HandleError(context, LogLevel.Info, HttpStatusCode.NotAcceptable);
            }
            else if (context.Exception is IOException)
            {
                HandleError(context, LogLevel.Fatal, HttpStatusCode.ServiceUnavailable);
            }
            else if (context.Exception is DbUpdateConcurrencyException)
            {
                HandleError(context, LogLevel.Debug, HttpStatusCode.Conflict);
            }
            else
            {
                HandleError(context, LogLevel.Error, HttpStatusCode.InternalServerError);
            }

            //base.OnException(context);
        }

        private static void HandleError(HttpActionExecutedContext context, LogLevel logLevel, HttpStatusCode httpStatusCode)
        {
            var returnValueException = new ApiErrorExcpetion(GenericApiErrorMessage(), context.Exception.Message);
            Log(logLevel, context);
            context.Response = context.Request.CreateResponse(httpStatusCode, returnValueException, new JsonMediaTypeFormatter());
        }

        private static string GenericApiErrorMessage()
        {
            return ConfigurationHelper.InstanceIsProduction()
                                    ? Constants.ApiGenericErrorMessage
                                    : $"{Constants.ApiGenericErrorMessage} Please see System Logs for more details.";
        }

        private static void Log(LogLevel logLevel, HttpActionExecutedContext context)
        {
            var logger = LogManager.GetLogger(context.Exception.Source);
            logger.Log(logLevel, context.Exception);
        }

        private static void Log(LogLevel logLevel, Exception data)
        {
            var logger = LogManager.GetLogger(data.Source);
            logger.Log(logLevel, data);
        }
    }
}
