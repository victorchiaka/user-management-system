using Microsoft.EntityFrameworkCore;
using UMS.Features;
using UMS.Persistence;

namespace UMS.Contracts;

public class UserService : IUserService
{
    private readonly UserDbContext _dbContext;

    public UserService(UserDbContext dbContext)
    {
        this._dbContext = dbContext;
    }
    
    public async Task CreateUser(string username, string emailAddress, string password, string passwordHash)
    {
        _dbContext.Users.Add(new User
        {
            Username = username,
            EmailAddress = emailAddress,
            PasswordHash = passwordHash,
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

    public async Task ChangeUserName(long userId, string newUsername)
    {
        User? user = await _dbContext.Users.SingleOrDefaultAsync(user => user.Id == userId);

        if (user is null)
        {
            return;
        }

        user.Username = newUsername;
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task ChangeUserEmail(long userId, string newEmailAddress)
    {
        User? user = await _dbContext.Users.SingleOrDefaultAsync(user => user.Id == userId);

        if (user is null)
        {
            return;
        }

        user.EmailAddress = newEmailAddress;
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task ChangeUserPassword(long userId, string newPasswordHash)
    {
        User? user = await _dbContext.Users.SingleOrDefaultAsync(user => user.Id == userId);

        if (user is null)
        {
            return;
        }

        user.PasswordHash = newPasswordHash;
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteUserFromDb(long userId)
    {
        User? user = await _dbContext.Users.SingleOrDefaultAsync(user => user.Id == userId);

        if (user is null)
        {
            return;
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }
}