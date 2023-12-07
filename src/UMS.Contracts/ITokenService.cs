namespace UMS.Contracts;

public interface ITokenService
{
    // Task<string> CreateJwtToken(long userId);
    string CreateJwtToken(long userId);
}