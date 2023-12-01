using UMS.Features;

namespace UMS.Contracts;

public interface IUserService
{
    Task CreateUser(string username, string emailAddress, string passwordHash, DateTime createdAt);

    Task<User?> GetUserById(long userId);
    
    Task<User?> GetUserByEmail(long emailAddress);
    
    Task ChangeUserName(long userId, string newUsername);
    
    Task ChangeUserEmail(long userId, string newEmail);
    
    Task ChangeUserPassword(long userId, string newPassword);
}