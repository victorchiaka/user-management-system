using UMS.Features;

namespace UMS.Contracts;

public interface IUserService
{
    Task CreateUser(string username, string emailAddress, string password, string passwordHash);

    Task<User?> GetUserById(long userId);
    
    Task<User?> GetUserByEmail(string emailAddress);
    
    Task ChangeUserName(long userId, string newUsername);
    
    Task ChangeUserEmail(long userId, string newEmail);
    
    Task ChangeUserPassword(long userId, string newPassword);
}