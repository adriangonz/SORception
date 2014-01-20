using ScrapWeb.Exceptions;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace ScrapWeb.Filters 
{
    public class ExceptionHandlingFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is ServiceException)
            {
                var exception = context.Exception as ServiceException;
                throw new HttpResponseException(new HttpResponseMessage(exception.StatusCode)
                {
                    Content = new StringContent(exception.Message),
                    ReasonPhrase = "Exception"
                });
            }

            //Log Critical errors
            Debug.WriteLine(context.Exception);

            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An error occurred, please try again or contact the administrator."),
                ReasonPhrase = "Critical Exception"
            });
        }
    }
}