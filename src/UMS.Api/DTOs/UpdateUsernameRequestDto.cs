namespace UMS.Api.DTOs;

public class UpdateUsernameRequestDto
{
    public string EmailAddress { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string NewUsername { get; set; } = string.Empty;
}