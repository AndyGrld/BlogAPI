using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Models
{
    [Table("Blogs")]
    public class Blog
    {
        public int BlogId { get; set; }
        public int? Userid { get; set; }
        public User? User { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime DataPosted { get; set; }
    }
}