namespace UMS.Api.DTOs;

public class UpdatePasswordRequestDto
{
    public string EmailAddress { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}