using System.Net.Sockets;
using AutoMapper;
using BlogAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext db;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        public UserController(AppDbContext _db, IUserRepository userRepository,
            IMapper mapper)
        {
            db = _db;
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var users = await userRepository.GetAllAsync();
            // var usersDto = users.Select(u => new UserDto()
            // {
            //     UserId = u.UserId,
            //     Username = u.Username,
            //     Email = u.Email,
            //     Blogs = u.Blogs
            // }).ToList();
            var usersDto = mapper.Map<List<UserDto>>(users);
            return Ok(usersDto);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var user = await userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUser([FromRoute]int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }
            var user = await userRepository.DeleteByIdAsync(id);
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<User>> CreateUser([FromBody]UserCreateDto usertoCreate)
        {
            var user = mapper.Map<User>(usertoCreate);
            user = await userRepository.CreateAsync(user);
            var userDto = mapper.Map<UserDto>(user);
            return CreatedAtAction(nameof(GetById), new {id = userDto.UserId}, userDto);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateUser([FromRoute]int id, [FromBody]User? user)
        {
            try
            {
                if(id != user.UserId || id <= 0)
                {
                    return BadRequest();
                }
                user = await userRepository.UpdateByIdAsync(id, user);
                return NoContent();
            }
            // For errors during updating such as errors due to foreign constraints
            catch(DbUpdateException e)
            {
                Console.WriteLine($"Database Update Error: {e}");
                return StatusCode(500, new {Message = "An error occured during update."});
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> LoginUser([FromBody] UserLoginDto userLogin)
        {
            var user = await userRepository.AuthenticateUser(userLogin);
            if (user == null)
            {
                return BadRequest(new {Message = "Wrong email or password"});
            }
            var userDto = mapper.Map<UserDto>(user);
            return Ok(new {Message = "Logged in successfully", Data = userDto});
        }

        [HttpPost("{id}/logout")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult LogoutUser(int id)
        {
            if (Session.UserId != id || Session.UserId <= 0)
            {
                return BadRequest(new {Message = "User not logged in"});
            }
            Session.UserId = 0;
            Session.CurrentUser = null;
            return Ok(new {Message = "Logged out successfully"});
        }
    }
}