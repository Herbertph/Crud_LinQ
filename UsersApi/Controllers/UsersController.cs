using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersApi.Models;
using UsersApi.Data;

namespace UsersApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // Get all Users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            // LINQ: Using ToListAsync() to asynchronously get all users from the database
            var users = await _context.Users.ToListAsync(); 
            return Ok(users);
        }

        // Add a new User
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest("Invalid username.");

            var user = new User { Name = name };

            // LINQ: Adding a new user to the DbSet. EF Core manages this operation internally using LINQ.
            _context.Users.Add(user);
            await _context.SaveChangesAsync(); // Asynchronously save changes to the database

            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
        }

        // Update a User's name
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] string newName)
        {
            // LINQ: Using Where() to find a user by ID
            var user = await _context.Users
                .Where(u => u.Id == id) // LINQ: Filter by user ID
                .FirstOrDefaultAsync(); // LINQ: Get the first matching user or null if not found

            if (user == null)
                return NotFound("User not found.");

            user.Name = newName;

            // LINQ: EF Core will track the changes and update the user record
            await _context.SaveChangesAsync(); 

            return Ok(user);
        }

        // Delete a User
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // LINQ: Using Where() to find the user by ID
            var user = await _context.Users
                .Where(u => u.Id == id) // LINQ: Filter by user ID
                .FirstOrDefaultAsync(); // LINQ: Get the first matching user or null if not found

            if (user == null)
                return NotFound("User not found.");

            // LINQ: Using Remove() to delete the user from the DbSet (EF Core handles this with LINQ)
            _context.Users.Remove(user); 
            await _context.SaveChangesAsync(); // Save changes asynchronously to persist deletion

            return NoContent();
        }

        // Search Users by name (example of LINQ filter)
        [HttpGet("search")]
        public async Task<IActionResult> GetUsersByName(string name)
        {
            // LINQ: Using Where() to filter users by name, with Contains() to find partial matches
            var users = await _context.Users
                .Where(u => u.Name.Contains(name)) // LINQ: Filter users by partial name match
                .ToListAsync(); // LINQ: Convert the filtered results to a list

            return Ok(users);
        }

    }
}
