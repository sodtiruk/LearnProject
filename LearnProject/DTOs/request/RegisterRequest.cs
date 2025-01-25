using System.ComponentModel.DataAnnotations;

namespace LearnProject.Dtos.request
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "The username field is required.")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "The password field is required.")]
        public required string Password { get; set; }

    }
}
