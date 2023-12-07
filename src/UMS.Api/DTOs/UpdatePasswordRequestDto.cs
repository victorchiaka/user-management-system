namespace UMS.Api.DTOs;

public class UpdatePasswordDto
{
    public string EmailAddress { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}