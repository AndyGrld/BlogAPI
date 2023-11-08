using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Models
{
    public class BlogCreateDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}