using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UMS.Contracts;
using UMS.Persistence;


namespace UMS.Integration.Spec.Hooks;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ServiceDescriptor? descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<UserDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            
            services.AddSingleton<DbContextOptions<UserDbContext>>(options =>
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
                dbContextOptionsBuilder.UseInMemoryDatabase("InMemoryDbForTesting");
                return dbContextOptionsBuilder.Options;
            });

            
        });

        base.ConfigureWebHost(builder);
    }
}