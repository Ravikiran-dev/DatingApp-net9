using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entity;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(AppUser user)
    {
        var tokenKey = config["TokenKey"] ?? throw new Exception("cannot access token key from appsettings") ; // Retrieve the token key from configuration
        if(tokenKey.Length < 64){
            throw new Exception("token key must be at least 64 characters long"); // Ensure the token key is at least 64 characters long
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)); // Create a symmetric security key from the token key

        var claims = new List<Claim> // Create a list of claims for the token
        {
            new (ClaimTypes.NameIdentifier, user.UserName) // Add the user's username as a claim
        };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature); // Create signing credentials using the symmetric key and HMAC SHA512 algorithm

        var tokenDescriptor = new SecurityTokenDescriptor // Create a token descriptor
        {
            Subject = new ClaimsIdentity(claims), // Set the claims identity
            Expires = DateTime.UtcNow.AddDays(7), // Set the token expiration time
            SigningCredentials = creds // Set the signing credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler(); // Create a new JWT security token handler
        var token = tokenHandler.CreateToken(tokenDescriptor); // Create the token using the token descriptor

        return tokenHandler.WriteToken(token); // Write the token to a string and return it


    }
}
