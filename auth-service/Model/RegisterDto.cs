using System.ComponentModel.DataAnnotations;


namespace AuthService.Models
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; } = default!;

        [Required, EmailAddress]
        public string Email { get; set; } = default!;
        
        [Required, MinLength(6)]
        public string Password { get; set; } = default!;
        
        public string Role { get; set; } = "User";
    }
}
