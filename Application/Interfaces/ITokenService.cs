using Application.DTOs;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(UserTokenDto dto, string secret, string issuer, string audience, int expireMinutes);
    }
}
