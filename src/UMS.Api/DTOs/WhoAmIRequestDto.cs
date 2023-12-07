namespace UMS.Api.DTOs;

public class WhoAmIRequestDto
{
    public string EmailAddress { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}