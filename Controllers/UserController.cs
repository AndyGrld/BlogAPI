using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext db;
        public UserController(AppDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var users = await db.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (id < 0 || user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (id < 0 || user == null)
            {
                return BadRequest();
            }
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<User>> CreateUser([FromBody]User user)
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new {id = user.UserId}, user);
        }
    }
}