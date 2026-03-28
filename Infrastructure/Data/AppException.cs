using Application.Response;
using System.Net;

namespace Infrastructure.Data
{
    public class AppException : Exception
    {
        public AppException() { }

        public AppException(HttpStatusCode statusCode, string errorMsg) : base(errorMsg) {
            StatusCode = statusCode;
        }

        public AppException(HttpStatusCode statusCode, ErrorList error) {
            StatusCode = statusCode;
            CustomErrResp = error;
        }

        public HttpStatusCode StatusCode { get; set; }

        public ErrorList? CustomErrResp { get; set; }
    }
}
