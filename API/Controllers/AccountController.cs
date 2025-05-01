using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTO;
using API.Entity;
using API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")] // api/account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.UserName)) // Check if user already exists
        {
            return BadRequest("Username is taken"); // Return 400 if username is taken
        }
        using var hmac = new HMACSHA512();
        var user = new AppUser
        {
            UserName = registerDto.UserName.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        context.Users.Add(user); // Add the new user to the context
        await context.SaveChangesAsync(); // Save changes to the database
        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user) // Create a token for the new user
        };
    }

    [HttpPost("login")] // api/account/login
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower()); // Find user by username

        if (user == null) return Unauthorized("Invalid username"); // Return 401 if user not found
        using var hmac = new HMACSHA512(user.PasswordSalt); // Create a new HMACSHA512 instance with the user's password salt
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password)); // Compute the hash of the provided passwordif
        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) // Compare the computed hash with the stored hash
            {
                return Unauthorized("Invalid password"); // Return 401 if password is invalid
            }
        }
        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user) // Create a token for the user
        }; // Return the user if login is successful
    }
    private async Task<bool> UserExists(string username)
    {
        return await context.Users.AnyAsync(x => x.UserName == username.ToLower()); // Check if user exists in the database
    }
}
