using API.Data;
using API.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")] // /api/users
    public class UsersController(DataContext context) : BaseApiController
    {


        [AllowAnonymous]
        // Add your actions here
        [HttpGet] // /api/users
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await context.Users.ToListAsync(); // Fetch all users from the database
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id:int}")] // /api/users/{id}
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user = await context.Users.FindAsync(id); // Fetch user by ID from the database
            if (user == null)
            {
                return NotFound(); // Return 404 if user not found
            }
            return user;
        }
    }
}
