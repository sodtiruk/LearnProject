using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Filters;

namespace LearnProject.DTOs.request
{
    public class LoginRequest
    {

        [Required(ErrorMessage = "The username field is required.")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "The password field is required.")]
        public required string Password { get; set; }
    }

    public class LoginRequestExample : IExamplesProvider<LoginRequest>
    {
        public LoginRequest GetExamples()
        {
            return new LoginRequest
            {
                Username = "admin",
                Password = "1234"
            };
        }
    }
}