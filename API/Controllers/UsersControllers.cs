using System.Security.Claims;
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
    public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
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

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Get the username from the token
            if(userName == null)
            {
                return BadRequest("No username found in Token"); // Return 401 if user is not authorized
            }

            var user = await userRepository.GetUserByUsernameAsync(userName); // Fetch user by username from the database
            if (user == null)
            {
                return NotFound("Could not find users"); // Return 404 if user not found
            }

            mapper.Map(memberUpdateDto, user); // Map the updated data to the user entity

            userRepository.Update(user); // Update the user in the repository

            if(await userRepository.SaveAllAsync())
            {
                return NoContent(); // Return 204 No Content if update is successful
            }

            return BadRequest("Failed to update user"); // Return 400 Bad Request if update fails

        }
            
    }
}
