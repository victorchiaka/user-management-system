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
    private readonly IAuthService _authService;
    
    public UserController(IUserService userService, IAuthService authService)
    {
        this._userService = userService;
        this._authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserRegisterDto registerUserDto)
    {
        User? user = await _userService.GetUserByEmail(registerUserDto.EmailAddress);

        if (user is not null)
        {
            return BadRequest("User with this email already exist");
        }
        
        await _userService.CreateUser(
            registerUserDto.Username,
            registerUserDto.EmailAddress,
            registerUserDto.Password,
            _authService.HashPassword(registerUserDto.Password)
        );
        return StatusCode(201);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUsername(UpdateUsernameDto updateUsernameDto)
    {
        User? user = await _userService.GetUserByEmail(updateUsernameDto.EmailAddress);

        if (user is null)
        {
            return NotFound("Wrong email address");
        }
        
        if (!_authService.IsVerifiedPassword(updateUsernameDto.Password, user.PasswordHash))
        {
            return BadRequest("Incorrect password");
        }
        
        await _userService.ChangeUserName(user.Id, updateUsernameDto.NewUsername);
        return Ok();
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateEmailAddress(UpdateEmailAddressDto updateEmailAddressDto)
    {
        User? user = await _userService.GetUserByEmail(updateEmailAddressDto.EmailAddress);

        if (user is null)
        {
            return NotFound("Wrong email address");
        }
        
        if (!_authService.IsVerifiedPassword(updateEmailAddressDto.Password, user.PasswordHash))
        {
            return BadRequest("Incorrect password");
        }
        
        await _userService.ChangeUserEmail(user.Id, updateEmailAddressDto.NewEmailAddress);
        return Ok();
    }
}