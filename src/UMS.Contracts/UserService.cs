using Microsoft.EntityFrameworkCore;
using UMS.Features;
using UMS.Persistence;
using BCrypt.Net;

namespace UMS.Contracts;

public class UserService : IUserService
{
    private readonly UserDbContext _dbContext;

    public UserService(UserDbContext dbContext)
    {
        this._dbContext = dbContext;
    }
    
    public async Task CreateUser(string username, string emailAddress, string password)
    {
        _dbContext.Users.Add(new User
        {
            Username = username,
            EmailAddress = emailAddress,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            CreatedAt = DateTime.UtcNow
        });

        await _dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetUserById(long userId)
    {
        return await _dbContext.Users.SingleOrDefaultAsync(user => user.Id == userId);
    }

    public async Task<User?> GetUserByEmail(string emailAddress)
    {
        return await _dbContext.Users.SingleOrDefaultAsync(user => user.EmailAddress == emailAddress);
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