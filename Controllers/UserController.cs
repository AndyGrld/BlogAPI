using System.Net.Sockets;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BCrypt.Net;
using BlogAPI.Repository.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext db;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IConfiguration iConfiguration;
        public UserController(AppDbContext _db, IUserRepository userRepository,
            IMapper mapper, IConfiguration iConfiguration)
        {
            db = _db;
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.iConfiguration = iConfiguration;
        }

        [HttpGet, Authorize(Roles = "User")]
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
            usertoCreate.Password = BCrypt.Net.BCrypt.HashPassword(usertoCreate.Password);
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
        public async Task<ActionResult> UpdateUser([FromRoute]int id, [FromBody]UserUpdateDto userDto)
        {
            try
            {
                if(id != userDto.UserId || id <= 0)
                {
                    return BadRequest();
                }
                var user = mapper.Map<User>(userDto);
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
            var user = await db.Users
                .FirstOrDefaultAsync(u => u.Email == userLogin.Email);
            if (user == null)
            {
                return BadRequest(new {Message = "Wrong email, please try again"});
            }
            if (!BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password))
            {
                return BadRequest(new {Message = "Wrong password, please try again"});
            }
            var userDto = mapper.Map<UserDto>(user);
            string token = CreateToken(user);
            return Ok(new {Message = $"Logged in successfully", Data = token});
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                iConfiguration.GetSection("AppSettings:Token").Value!
            ));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}