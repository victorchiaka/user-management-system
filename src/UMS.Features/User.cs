using System.ComponentModel.DataAnnotations.Schema;

namespace UMS.Features;

public class User
{
    public long Id { get; set; }
    
    public string Username { get; set; }
    
    public string EmailAddress { get; set; }
    
    public string PasswordHash { get; set; }
    
    public DateTime CreatedAt { get; set; }
}