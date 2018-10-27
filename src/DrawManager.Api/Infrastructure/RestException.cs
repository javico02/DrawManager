using System;
using System.Net;

namespace DrawManager.Api.Infrastructure
{
    public class RestException : Exception
    {
        public object Errors { get; private set; }
        public HttpStatusCode Code { get; private set; }

        public RestException(HttpStatusCode code, object errors = null)
        {
            Code = code;
            Errors = errors;
        }
    }
}
