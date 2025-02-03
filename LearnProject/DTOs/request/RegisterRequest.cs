using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Filters;

namespace LearnProject.Dtos.request
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "The username field is required.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "The password field is required.")]
        public required string Password { get; set; }
    }

    public class RegisterRequestExample : IExamplesProvider<RegisterRequest>
        {
            public RegisterRequest GetExamples()
            {
                return new RegisterRequest
                {
                    Email = "admin",
                    Password = "1234"
                };
            }
        }
}


