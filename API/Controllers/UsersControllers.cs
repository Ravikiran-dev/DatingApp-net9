using API.Data;
using API.DTO;
using API.Entity;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")] // /api/users
    public class UsersController(IUserRepository userRepository) : BaseApiController
    {

        // Add your actions here
        [HttpGet] // /api/users
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await userRepository.GetMembersAsync(); // Fetch all users from the database

            return Ok(users);
        }

        
        [HttpGet("{username}")] // /api/users/{id}
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = await userRepository.GetMemberAsync(username); // Fetch user by ID from the database
            if (user == null)
            {
                return NotFound(); // Return 404 if user not found
            }

            return user; // Map the user to MemberDto and return it  
        }
    }
}
