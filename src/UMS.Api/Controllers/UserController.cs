using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UMS.Api.DTOs;
using UMS.Contracts;
using UMS.Features;

namespace UMS.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authenticationService;
    
    public UserController(IUserService userService, IAuthenticationService authenticationService)
    {
        this._userService = userService;
        this._authenticationService = authenticationService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterUserRequestDto requestRegisterUserDto)
    {
        User? user = await _userService.GetUserByEmail(requestRegisterUserDto.EmailAddress);

        if (user is not null)
        {
            return BadRequest("User with this email already exist");
        }
        
        await _userService.CreateUser(
            requestRegisterUserDto.Username,
            requestRegisterUserDto.EmailAddress,
            requestRegisterUserDto.Password,
            _authenticationService.HashPassword(requestRegisterUserDto.Password)
        );
        
        return StatusCode(201, "Successfully registered user");
    }
    
    [HttpPost]
    public async Task<ActionResult<AuthenticationCredentialsDto>> Login(LoginUserRequestDto loginUserRequestDto)
    {
        string? jwt = await _authenticationService.AuthenticateUser(loginUserRequestDto.EmailAddress, loginUserRequestDto.Password);

        if (jwt is null)
        {
            return BadRequest("Invalid email or password");
        }

        return Ok(new AuthenticationCredentialsDto { Jwt = jwt });
    }
    
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateUsername(UpdateUsernameRequestDto updateUsernameRequestDto)
    {
        User? user = await _userService.GetUserByEmail(updateUsernameRequestDto.EmailAddress);

        if (user is null)
        {
            return NotFound("User with this email doesn't exist");
        }
        
        if (!_authenticationService.IsVerifiedPassword(updateUsernameRequestDto.Password, user.PasswordHash))
        {
            return BadRequest("Incorrect password");
        }
        
        await _userService.ChangeUserName(user.Id, updateUsernameRequestDto.NewUsername);
        
        return Ok("Successfully update username");
    }
    
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateEmailAddress(UpdateEmailAddressRequestDto updateEmailAddressRequestDto)
    {
        User? user = await _userService.GetUserByEmail(updateEmailAddressRequestDto.EmailAddress);

        if (user is null)
        {
            return NotFound("User with this email doesn't exist");
        }
        
        if (!_authenticationService.IsVerifiedPassword(updateEmailAddressRequestDto.Password, user.PasswordHash))
        {
            return BadRequest("Incorrect password");
        }
        
        await _userService.ChangeUserEmail(user.Id, updateEmailAddressRequestDto.NewEmailAddress);
        
        return Ok("Successfully updated email");
    }
    
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdatePassword(UpdatePasswordRequestDto updatePasswordRequestDto)
    {
        User? user = await _userService.GetUserByEmail(updatePasswordRequestDto.EmailAddress);

        if (user is null)
        {
            return NotFound("User with this email doesn't exist");
        }
        
        await _userService.ChangeUserPassword(user.Id, _authenticationService.HashPassword(updatePasswordRequestDto.NewPassword));
        
        return Ok("Successfully updated password");
    }
    
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(DeleteUserRequestDto deleteUserRequestDto)
    {
        User? user = await _userService.GetUserByEmail(deleteUserRequestDto.EmailAddress);
        
        if (user is null)
        {
            return NotFound("User with this email doesn't exist");
        }

        await _userService.DeleteUserFromDb(user.Id);
        
        return Ok("Successfully deleted user");
    }
    
}