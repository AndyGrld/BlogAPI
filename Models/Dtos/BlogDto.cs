using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Models
{
    public class BlogDto
    {
        public User? User { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}