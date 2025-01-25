using System.ComponentModel.DataAnnotations;

namespace LearnProject.DTOs.request
{
    public class LoginRequest
    {

        [Required(ErrorMessage = "The username field is required.")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "The password field is required.")]
        public required string Password { get; set; }
    }
}