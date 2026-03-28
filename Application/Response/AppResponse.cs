using System.Net;

namespace Application.Response
{
    public class AppResponse
    {
        public bool Success { get; protected set; }

        public string Message { get; protected set; } = string.Empty;

        public List<string> Errors { get; protected set; } = [];

        public string Token { get; protected set; } = string.Empty;

        public int Status { get; protected set; }

        public static AppResponse Ok(string message = "", string token = "") =>
            new() {
                Success = true,
                Token = token,
                Message = message,
                Status = (int) HttpStatusCode.OK
            };

        public static AppResponse Fail(string message, List<string>? errors = null, HttpStatusCode? statusCode = null) =>
            new() { 
                Success = false, 
                Message = message, 
                Errors = errors ?? [], 
                Status = (int) (statusCode ?? HttpStatusCode.BadRequest) 
            }; 
    }

    public class AppResponse<T> : AppResponse
    {
        public T? Data { get; private set; }

        public static AppResponse<T> Ok(T data, string message = "", string token = "")
        {
            return new AppResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Token = token,
                Status = (int) HttpStatusCode.OK
            };
        }
    }
}
