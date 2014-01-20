using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ScrapWeb.Exceptions
{
    public class ServiceException : Exception
    {
        private IEnumerable<String> errors;
        private String error;
        private HttpStatusCode code;

        public ServiceException(IEnumerable<string> errors, HttpStatusCode code = HttpStatusCode.InternalServerError)
        {
            this.errors = errors;
            this.error = null;
            this.code = code;
        }

        public ServiceException(String message, HttpStatusCode code = HttpStatusCode.InternalServerError)
        {
            this.error = message;
            this.errors = null;
            this.code = code;
        }

        public override string Message
        {
            get
            {
                if (errors == null)
                    return this.error;

                var errorMessage = "[";
                foreach(var error in this.errors) {
                    errorMessage += error + ",";
                }
                errorMessage += "]";
                return errorMessage;
            }
        }

        public HttpStatusCode StatusCode 
        {
            get
            {
                return code;
            }
        }
    }
}