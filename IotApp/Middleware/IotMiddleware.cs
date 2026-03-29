using Application.Common.Base64;
using Application.Common.Exceptions;
using Application.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IotApp.Middleware
{
    public class IotMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext ctx)
        {
            try
            {
                if (ctx.Request.Method == "OPTIONS")
                {
                    await _next(ctx);
                    return;
                }

                var endpoint = ctx.GetEndpoint();
                var allowAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;

                if (allowAnonymous)
                {
                    await _next(ctx);
                    return;
                }

                var path = ctx.Request.Path.Value?.ToLower();

                // Permitir SignalR sin validar token
                if (path!.StartsWith("/ws"))
                {
                    await _next(ctx);
                    return;
                }

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

                var payload = ValidTokenAndDeserialize(token);
                if (payload == null)
                    throw new AppException(HttpStatusCode.Unauthorized, "Token invalido");

                // Split a los roles.
                var rolesToken = payload.Role.Split(",", StringSplitOptions.RemoveEmptyEntries);

                var claims = new List<Claim>
                {
                    new (ClaimTypes.Name, payload.Sub),
                };

                foreach (var role in rolesToken)
                {
                    claims.Add(new (ClaimTypes.Role, role.Trim())); // cada rol como claim
                }

                ctx.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "ManualJwt"));

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

        private static ValidTokenResponse? ValidTokenAndDeserialize(string token)
        {
            var parts = token.Split('.');
            if (parts.Length != 3) return null;

            var header = parts[0];
            var payload = parts[1];
            var SignatureFromSubstring = parts[2];

            var unsignedToken = $"{header}.{payload}";

            var _secret = "mi_secret_key_qwertyuiopasdfghjklñzxcvbnm";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(unsignedToken));

            var signatureFromUnsignedToken = Base64Utility.Base64UrlEncode(hash);

            if (SignatureFromSubstring != signatureFromUnsignedToken) return null;

            var payloadJson = Encoding.UTF8.GetString(Base64Utility.Base64UrlDecode(payload));

            if (payloadJson.Contains("\"exp\":"))
            {
                var expValue = Base64Utility.ExtractExp(payloadJson);
                var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                if (now > expValue) return null;
            }

            var jwtPayload = JsonSerializer.Deserialize<ValidTokenResponse>(payloadJson);

            return jwtPayload;
        }


    }

    public class ValidTokenResponse
    {
        [JsonPropertyName("sub")]
        public string Sub { get; set; } = string.Empty;

        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("exp")]
        public long Exp { get; set; }
    }
}
