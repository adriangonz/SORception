using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrapWeb.Exceptions
{
    public class ServiceException : Exception
    {
        private IEnumerable<String> errors;

        public ServiceException(IEnumerable<string> errors)
        {
            this.errors = errors;
        }

        public override string Message
        {
            get
            {
                var errorMessage = "[";
                foreach(var error in this.errors) {
                    errorMessage += error + ",";
                }
                errorMessage += "]";
                return errorMessage;
            }
        }
    }
}