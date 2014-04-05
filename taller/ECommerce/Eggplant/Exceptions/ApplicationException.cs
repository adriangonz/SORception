using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Eggplant.Exceptions
{
    public class ApplicationLayerException : Exception
    {
        private IEnumerable<String> errors;
        private String error;
        private HttpStatusCode code;

        public ApplicationLayerException(HttpStatusCode code, IEnumerable<string> errors)
        {
            this.errors = errors;
            this.error = null;
            this.code = code;
        }

        public ApplicationLayerException(HttpStatusCode code, String message)
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