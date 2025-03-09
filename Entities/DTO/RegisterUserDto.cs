using System.ComponentModel.DataAnnotations;

namespace Entities.DTO
{
    public class RegisterUserDto
    {
        [Required]
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword {  get; set; }
        
    }
}
