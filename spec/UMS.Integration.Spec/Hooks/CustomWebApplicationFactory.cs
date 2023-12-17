using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UMS.Persistence;

namespace UMS.Integration.Spec.Hooks;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    public string DefaultUserId { get; set; } = "";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<UserDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContextFactory<UserDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<UserDbContext>();

                db.Database.EnsureCreated();

                Utilities.SeedData(db);
            }

        });

        builder.ConfigureTestServices(services =>
        {

            services.Configure<TestAuthenticationHandlerOptions>(options => options.DefaultUserId = DefaultUserId);

            services.AddAuthentication(TestAuthenticationHandler.AuthenticationScheme)
                .AddScheme<TestAuthenticationHandlerOptions, TestAuthenticationHandler>(TestAuthenticationHandler.AuthenticationScheme, options => { });
        });
    }
}