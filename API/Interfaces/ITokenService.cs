using System;
using API.Entity;

namespace API.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user); // Method to create a token for a user

}
