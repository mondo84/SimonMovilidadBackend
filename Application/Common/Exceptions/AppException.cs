using Application.Response;
using System.Net;

namespace Application.Common.Exceptions
{
    public class AppException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ErrorList? Error { get; }

        public AppException() { }

        public AppException(HttpStatusCode statusCode, string message) : base(message) {
            StatusCode = statusCode;
        }

        public AppException(HttpStatusCode statusCode, ErrorList error) : base(error?.Message)
        {
            StatusCode = statusCode;
            Error = error;
        }
        
    }
}
