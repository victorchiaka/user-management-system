using Microsoft.AspNetCore.Mvc;
using UMS.Contracts;

namespace UMS.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        this._userService = userService;
    }
}