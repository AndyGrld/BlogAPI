using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Models
{
    public class UserCreateDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}