using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Filters;

namespace LearnProject.DTOs.request
{
    public class LoginRequest
    {

        [Required(ErrorMessage = "The username field is required.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "The password field is required.")]
        public required string Password { get; set; }
    }

    public class LoginRequestExample : IExamplesProvider<LoginRequest>
    {
        public LoginRequest GetExamples()
        {
            return new LoginRequest
            {
                Email = "admin",
                Password = "1234"
            };
        }
    }
}