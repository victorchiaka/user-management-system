using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace UMS.Integration.Spec.Hooks;

public class TestAuthenticationHandlerOptions : AuthenticationSchemeOptions
{
    public string DefaultUserId { get; set; } = null!;
}

public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationHandlerOptions>
{
    public const string UserId = "UserId";
    public const string AuthenticationScheme = "Test";
    private readonly string _defaultUserId;

    public TestAuthenticationHandler(IOptionsMonitor<TestAuthenticationHandlerOptions> options, ILoggerFactory logger, UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _defaultUserId = options.CurrentValue.DefaultUserId;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Test user") };

        if (Context.Request.Headers.TryGetValue(UserId, out var userId))
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId[0]!));
        }
        else
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, _defaultUserId));
        }
        
        var identity = new ClaimsIdentity(claims, AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}