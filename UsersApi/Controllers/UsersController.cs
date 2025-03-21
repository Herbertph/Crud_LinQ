using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace UsersApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static List<string> users = new List<string> { "Alice", "Bob", "Charlie" };

        //Get Users
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(users.ToList()); // LINQ: ToList() to get a new list.
        }

        //Add a new User
        [HttpPost]
        public IActionResult AddUser([FromBody] string user)
        {
            if (string.IsNullOrEmpty(user))
                return BadRequest("Nome de usuário inválido.");

            users = users.Append(user).ToList(); // LINQ: Append() adds a new element to the list.
            return CreatedAtAction(nameof(GetUsers), new { name = user }, user);
        }

        // Upadate a User
        [HttpPut("{oldUser}")]
        public IActionResult UpdateUser(string oldUser, [FromBody] string newUser)
        {
            if (!users.Any(u => u.Equals(oldUser, StringComparison.OrdinalIgnoreCase)))
                return NotFound("Usuário não encontrado.");

            users = users.Select(u => u.Equals(oldUser, StringComparison.OrdinalIgnoreCase) ? newUser : u).ToList();
            // LINQ: Select() substitute the old user for the new one.
            
            return Ok(users);
        }

        // Delete an User
        [HttpDelete("{user}")]
        public IActionResult DeleteUser(string user)
        {
            if (!users.Any(u => u.Equals(user, StringComparison.OrdinalIgnoreCase)))
                return NotFound("Usuário não encontrado.");

            users = users.Where(u => !u.Equals(user, StringComparison.OrdinalIgnoreCase)).ToList();
            // LINQ: Where() filter the list to exclude the user.
            
            return Ok(users);
        }
    }
}