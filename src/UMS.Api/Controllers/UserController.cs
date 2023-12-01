using Microsoft.AspNetCore.Mvc;
using UMS.Api.DTOs;
using UMS.Contracts;
using UMS.Features;

namespace UMS.Api.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        this._userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserRegisterDto registerUserDto)
    {
        User? user = await _userService.GetUserByEmail(registerUserDto.EmailAddress);

        if (user is not null)
        {
            return BadRequest("User with this email already exist");
        }
        
        await _userService.CreateUser(registerUserDto.Username, registerUserDto.EmailAddress, registerUserDto.Password);

        return StatusCode(201);
    }
    
}