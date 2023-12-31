using System.Reflection.Metadata.Ecma335;
using AutoMapper;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly AppDbContext db;
        private readonly IMapper mapper;
        public BlogController(AppDbContext _db, IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllBlogs()
        {
            var blogs = await db.Blogs.ToListAsync();
            return Ok(blogs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBlogById([FromQuery] int id)
        {            
            if (await db.Blogs.FindAsync(id) == null)
                return NotFound();
            var blog = await db.Blogs.FirstOrDefaultAsync(b => b.BlogId == id);
            return Ok(blog);
        }

        [HttpPost]
        public async Task<ActionResult> CreateBlog(int userId, [FromBody] BlogCreateDto createBlog)
        {
            // get user creating blog
            var user = db.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var blog = mapper.Map<Blog>(createBlog);
            blog.Userid = userId;
            blog.DatePosted = DateTime.Today.Date;
            blog.TimePosted = DateTime.Now.TimeOfDay;
            db.Blogs.Add(blog);

            await db.SaveChangesAsync();
            var blogDto = mapper.Map<BlogDto>(blog);
            return CreatedAtAction(nameof(GetBlogById), new {id = blog.BlogId}, blogDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBlog(int id)
        {
            var blogToRemove = await db.Blogs.FindAsync(id);
            if (blogToRemove != null)
            {
                db.Blogs.Remove(blogToRemove);
                await db.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBlog(int id, Blog blog)
        {
            if (id != blog.BlogId)
            {
                return BadRequest();
            }
            db.Entry(blog).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return NoContent();
        }
    }
}