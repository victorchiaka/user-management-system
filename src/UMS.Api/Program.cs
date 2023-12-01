using DotNetEnv;
using UMS.Contracts;
using UMS.Persistence;
using Microsoft.EntityFrameworkCore;

Env.TraversePath().Load();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IConfigurationRoot configurationBuilder = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddDbContextFactory<UserDbContext>(options =>
{
    options.UseNpgsql(configurationBuilder.GetConnectionString(nameof(UserDbContext)));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserService>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseEndpoints(endpoint =>
{
    endpoint.MapControllers();
});

app.UseHttpsRedirection();
app.Run();