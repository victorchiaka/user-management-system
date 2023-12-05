using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace UMS.Api.Controllers;

public abstract class AbstractController : ControllerBase
{
    protected long? ContextUserId
    {
        get
        {
            Claim? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if (userIdClaim is not null && long.TryParse(userIdClaim.Value, out long userId))
            {
                return userId;
            }

            return null;
        }
    }
}