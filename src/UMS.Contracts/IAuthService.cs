namespace UMS.Contracts;

public interface IAuthService
{
    string HashPassword(string password);
    bool IsVerifiedPassword(string password, string passwordHash);
}