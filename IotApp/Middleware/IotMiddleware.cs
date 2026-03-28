using Application.Common.Base64;
using Application.Common.Exceptions;
using Application.Response;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace IotApp.Middleware
{
    public class IotMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext ctx)
        {
            try
            {
                var path = ctx.Request.Path.Value?.ToLower();
                

                if (path!.StartsWith("/swagger") || path.StartsWith("/api/auth"))
                {
                    await _next(ctx);
                    return;
                }

                if (!ctx.Request.Headers.TryGetValue("Authorization", out var authHeaderValues))
                    throw new AppException(HttpStatusCode.Unauthorized, "Token requeridos");

                var authHeader = authHeaderValues.FirstOrDefault();
                

                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                    throw new AppException(HttpStatusCode.Unauthorized, "Formato invalido. Se espera del Tipo. Bearer xxx");

                var token = authHeader.Substring("Bearer ".Length).Trim();

                var isValid = ValidToken(token);
                if (!isValid)
                    throw new AppException(HttpStatusCode.Unauthorized, "Token invalido");

                await _next(ctx);
                return;

            } catch(AppException appEx)
            {
                Console.WriteLine($"AppException: {appEx}");
                ctx.Response.StatusCode = (int)appEx.StatusCode;
                ctx.Response.ContentType = "application/json";

                var objAppEx = appEx.Error ?? new ErrorList { Message = appEx.Message };
                var objJson = AppResponse.Fail(objAppEx.Message, objAppEx.Errors, appEx.StatusCode);

                await ctx.Response.WriteAsJsonAsync(objJson);
            } catch(Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");

                ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await ctx.Response.WriteAsJsonAsync(new { Message = ex.Message ?? "Error inesperado" });
            }
        }

        private static bool ValidToken(string token)
        {
            var parts = token.Split('.');
            if (parts.Length != 3) return false;

            var header = parts[0];
            var payload = parts[1];
            var SignatureFromSubstring = parts[2];

            var unsignedToken = $"{header}.{payload}";

            var _secret = "mi_secret_key_qwertyuiopasdfghjklñzxcvbnm";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(unsignedToken));

            var signatureFromUnsignedToken = Base64Utility.Base64UrlEncode(hash);

            if (SignatureFromSubstring != signatureFromUnsignedToken) return false;

            var payloadJson = Encoding.UTF8.GetString(Base64Utility.Base64UrlDecode(payload));

            if (payloadJson.Contains("\"exp\":"))
            {
                var expValue = Base64Utility.ExtractExp(payloadJson);
                var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                if (now > expValue) return false;
            }

            return true;
        }
    }
}
