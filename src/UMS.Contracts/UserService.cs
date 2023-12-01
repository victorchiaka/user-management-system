using UMS.Features;

namespace UMS.Contracts;

public class UserService : IUserService
{
    public Task CreateUser(string username, string emailAddress, string passwordHash, DateTime createdAt)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetUserById(long userId)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetUserByEmail(long emailAddress)
    {
        throw new NotImplementedException();
    }

    public Task ChangeUserName(long userId, string newUsername)
    {
        throw new NotImplementedException();
    }

    public Task ChangeUserEmail(long userId, string newEmail)
    {
        throw new NotImplementedException();
    }

    public Task ChangeUserPassword(long userId, string newPassword)
    {
        throw new NotImplementedException();
    }
}