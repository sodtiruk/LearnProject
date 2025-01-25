using System.Net;
using LearnProject.Dtos.request;
using LearnProject.Dtos.response;
using LearnProject.DTOs.request;
using LearnProject.DTOs.response;
using LearnProject.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<BaseResponse<RegisteredResponse>>> Register(RegisterRequest registerRequest)
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
                return NotFound(new { message = ex.Message, StatusCode = HttpStatusCode.NotFound });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message, StatusCode = HttpStatusCode.NotFound });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", detail = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<BaseResponse<LoginResponse>>> Login(LoginRequest loginRequest)
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
                return NotFound(new { message = ex.Message, StatusCode = HttpStatusCode.NotFound });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message, StatusCode = HttpStatusCode.NotFound });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", detail = ex.Message });
            }
        }




    }
}
