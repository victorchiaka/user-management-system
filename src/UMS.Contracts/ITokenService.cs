namespace UMS.Contracts;

public interface ITokenService
{ 
    string CreateJwtToken(long userId);
}