using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Models
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public List<Blog>? Blogs { get; set; }
    }
}