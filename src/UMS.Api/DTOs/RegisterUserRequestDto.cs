namespace UMS.Api.DTOs;

public class RegisterUserRequestDto
{
    public string Username { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}