using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

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
        List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.Name, "Test user") };

        if (Context.Request.Headers.TryGetValue(UserId, out StringValues userId))
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId[0]!));
        }
        else
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, _defaultUserId));
        }

        ClaimsIdentity identity = new ClaimsIdentity(claims, AuthenticationScheme);
        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
        AuthenticationTicket ticket = new AuthenticationTicket(principal, AuthenticationScheme);

        AuthenticateResult result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}