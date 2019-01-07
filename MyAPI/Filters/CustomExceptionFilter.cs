namespace MyAPI.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Net;

    public class CustomExceptionFilter : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {

            HandleExceptionAsync(context);
            context.ExceptionHandled = true;
        }

        private static void HandleExceptionAsync(ExceptionContext context)
        {
            var exception = context.Exception;

            if (exception is System.ArgumentException)
                SetExceptionResult(context, exception, HttpStatusCode.BadRequest);
            else
                SetExceptionResult(context, exception, HttpStatusCode.InternalServerError);
        }

        private static void SetExceptionResult(
        ExceptionContext context,
        Exception exception,
        HttpStatusCode code)
        {
            context.Result = new JsonResult(new { error = exception.Message, status_code = code })
            {
                StatusCode = (int)code
            };
        }
    }
}
