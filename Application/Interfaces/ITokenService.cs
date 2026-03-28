namespace Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string secret, string issuer, string audience, int expireMinutes);
    }
}
