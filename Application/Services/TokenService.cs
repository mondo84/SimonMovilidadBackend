using Application.Common.Base64;
using Application.DTOs;
using Application.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Application.Services
{
    public class TokenService : ITokenService
    {
        public string GenerateToken(UserTokenDto dto, string secret, string issuer, string audience, int expireMinutes)
        {
            var header = new
            {
                alg = "HS256",
                typ = "JWT"
            };

            var payload = new
            {
                sub = dto.UserName,
                userId = dto.UserId,
                role = dto.Role,
                iss = issuer,
                aud = audience,
                exp = DateTimeOffset.UtcNow.AddMinutes(expireMinutes).ToUnixTimeSeconds(),
                iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            var headerJson = JsonSerializer.Serialize(header);
            var payloadJson = JsonSerializer.Serialize(payload);
            var headerBase64 = Base64Utility.Base64UrlEncode(Encoding.UTF8.GetBytes(headerJson));
            var payloadBase64 = Base64Utility.Base64UrlEncode(Encoding.UTF8.GetBytes(payloadJson));

            var unsignedToken = $"{headerBase64}.{payloadBase64}";

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(unsignedToken));

            var signature = Base64Utility.Base64UrlEncode(hash);

            return $"{unsignedToken}.{signature}";
        }
    }
}
