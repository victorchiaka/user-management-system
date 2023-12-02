namespace UMS.Contracts;

public interface IAuthenticationService
{
    string HashPassword(string password);
    bool IsVerifiedPassword(string password, string passwordHash);
}