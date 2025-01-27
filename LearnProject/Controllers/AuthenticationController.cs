using System.Net;
using LearnProject.Dtos.request;
using LearnProject.Dtos.response;
using LearnProject.DTOs.request;
using LearnProject.DTOs.response;
using LearnProject.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LearnProject.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;
        public AuthenticationController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register a new user", Description = "Registers a new user with a username and password.")]
        public async Task<ActionResult<BaseResponse<RegisteredResponse>>> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                return new BaseResponse<RegisteredResponse>
                {
                    Message = "Register successfully",
                    Data = await _authenticationService.Register(registerRequest),
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message, StatusCode = HttpStatusCode.BadRequest });
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message, StatusCode = HttpStatusCode.BadRequest });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", detail = ex.Message });
            }
        }

        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login user", Description = "Login a user with a username and password.")]
        public async Task<ActionResult<BaseResponse<LoginResponse>>> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                return new BaseResponse<LoginResponse>
                {
                    Message = "Login successfully",
                    Data = await _authenticationService.Login(loginRequest),
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message, StatusCode = HttpStatusCode.BadRequest });
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message, StatusCode = HttpStatusCode.BadRequest });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", detail = ex.Message });
            }
        }

    }
}
