using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace UMS.Persistence;

public class UserDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
    public UserDbContext CreateDbContext(string[] args)
    {
        Env.TraversePath().Load();
        
        IConfigurationRoot configurationRoot = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();
        
        DbContextOptionsBuilder<UserDbContext> optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();

        optionsBuilder.UseNpgsql(configurationRoot.GetConnectionString(nameof(UserDbContext)));

        return new UserDbContext(optionsBuilder.Options);
    }
}