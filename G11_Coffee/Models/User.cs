using System.ComponentModel.DataAnnotations;

namespace G11_Coffee.Models
{
    public class User
    {
        [Key]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
