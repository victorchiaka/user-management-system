namespace UMS.Api.DTOs;

public class UpdateEmailAddressDto
{
    public string EmailAddress { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string NewEmailAddress { get; set; } = string.Empty;
}