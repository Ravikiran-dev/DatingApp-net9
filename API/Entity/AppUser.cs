using System;

namespace API.Entity;

public class AppUser
{
    // Add properties and methods for the AppUser class here
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required byte[]  PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }

}
