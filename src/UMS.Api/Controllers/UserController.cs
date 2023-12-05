using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UMS.Api.DTOs;
using UMS.Contracts;
using UMS.Features;

namespace UMS.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/v1/[controller]/[action]")]
public class UserController : AbstractController
{
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authenticationService;
    
    public UserController(IUserService userService, IAuthenticationService authenticationService)
    {
        this._userService = userService;
        this._authenticationService = authenticationService;
    }

    [AllowAnonymous]
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
            _authenticationService.HashPassword(registerUserDto.Password)
        );
        
        return StatusCode(201, "Successfully registered user");
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<AuthenticationCredentialsDto>> Login(UserLoginDto userLoginDto)
    {
        string? jwt = await _authenticationService.AuthenticateUser(userLoginDto.EmailAddress, userLoginDto.Password);

        if (jwt is null)
        {
            return BadRequest("Invalid email or password");
        }

        return Ok(new AuthenticationCredentialsDto { Jwt = jwt });
}
    
    [HttpPut]
    public async Task<IActionResult> UpdateUsername(UpdateUsernameDto updateUsernameDto)
    {
        if (!ContextUserId.HasValue)
        {
            return Unauthorized();
        }
        
        User? user = await _userService.GetUserByEmail(updateUsernameDto.EmailAddress);

        if (user is null)
        {
            return NotFound("User with this email doesn't exist");
        }
        
        if (!_authenticationService.IsVerifiedPassword(updateUsernameDto.Password, user.PasswordHash))
        {
            return BadRequest("Incorrect password");
        }
        
        await _userService.ChangeUserName(user.Id, updateUsernameDto.NewUsername);
        
        return Ok("Successfully update username");
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateEmailAddress(UpdateEmailAddressDto updateEmailAddressDto)
    {
        if (!ContextUserId.HasValue)
        {
            return Unauthorized();
        }
        
        User? user = await _userService.GetUserByEmail(updateEmailAddressDto.EmailAddress);

        if (user is null)
        {
            return NotFound("User with this email doesn't exist");
        }
        
        if (!_authenticationService.IsVerifiedPassword(updateEmailAddressDto.Password, user.PasswordHash))
        {
            return BadRequest("Incorrect password");
        }
        
        await _userService.ChangeUserEmail(user.Id, updateEmailAddressDto.NewEmailAddress);
        
        return Ok("Successfully updated email");
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdatePassword(UpdatePasswordDto updatePasswordDto)
    {
        if (!ContextUserId.HasValue)
        {
            return Unauthorized();
        }
        
        User? user = await _userService.GetUserByEmail(updatePasswordDto.EmailAddress);

        if (user is null)
        {
            return NotFound("User with this email doesn't exist");
        }
        
        await _userService.ChangeUserPassword(user.Id, _authenticationService.HashPassword(updatePasswordDto.NewPassword));
        
        return Ok("Successfully updated password");
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(DeleteUserDto deleteUserDto)
    {
        if (!ContextUserId.HasValue)
        {
            return Unauthorized();
        }
        
        User? user = await _userService.GetUserByEmail(deleteUserDto.EmailAddress);
        
        if (user is null)
        {
            return NotFound("User with this email doesn't exist");
        }

        await _userService.DeleteUserFromDb(user.Id);
        
        return Ok("Successfully deleted user");
    }
}