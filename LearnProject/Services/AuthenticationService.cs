using LearnProject.Dtos.request;
using LearnProject.DTOs.request;
using LearnProject.DTOs.response;
using LearnProject.Models;
using LearnProject.Repositories;
using LearnProject.Utils;

namespace LearnProject.Services
{
    public class AuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationReponsitory;
        private readonly JwtUtil _jwtUtil; 

        public AuthenticationService(IAuthenticationRepository authenticationReponsitory, JwtUtil jwtUtil)
        {
            _authenticationReponsitory = authenticationReponsitory;
            _jwtUtil = jwtUtil;
        }

        public async Task<RegisteredResponse> Register(RegisterRequest registerRequest)
        {
            ValidateRegisterRequest(registerRequest);

            UserModel? user = await _authenticationReponsitory.GetUserByUsername(registerRequest);
            if (user != null)
            {
                throw new KeyNotFoundException("User already exists");
            }

            registerRequest.Password = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

            UserModel userModel = await SaveUserToDatabase(registerRequest);

            return new RegisteredResponse
            {
                Id = userModel.Id,
                Username = userModel.Username,
            };

        }

        private static void ValidateRegisterRequest(RegisterRequest registerRequest)
        {
            if (registerRequest.Username == null)
            {
                throw new ArgumentNullException(nameof(registerRequest), "Username is null");
            }

            if (registerRequest.Password == null)
            {
                throw new ArgumentNullException(nameof(registerRequest), "Password is null");
            }
        }

        private async Task<UserModel> SaveUserToDatabase(RegisterRequest registerRequest)
        {
            var newUser = new UserModel
            {
                Username = registerRequest.Username,
                Password = registerRequest.Password 
            };

            return await _authenticationReponsitory.AddUser(newUser);
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            ValidateLoginRequest(loginRequest);

            UserModel? userModel = await _authenticationReponsitory.GetUserByUsername(loginRequest);
            
            if (userModel == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            if (!isPasswordCorrect(loginRequest, userModel))
            {
                throw new UnauthorizedAccessException("Password is incorrect");
            }

            return new LoginResponse
            {
                Token = _jwtUtil.GenerateToken(userModel.Id.ToString())
            };

        }

        private static bool isPasswordCorrect(LoginRequest loginRequest, UserModel userModel)
        {
            return BCrypt.Net.BCrypt.Verify(loginRequest.Password, userModel.Password);
        }

        private static void ValidateLoginRequest(LoginRequest loginRequest)
        {

            if (loginRequest.Username == null)
            {
                throw new ArgumentNullException(nameof(loginRequest), "Username is null");
            }

            if (loginRequest.Password == null)
            {
                throw new ArgumentNullException(nameof(loginRequest), "Password is null");
            }

        }
    }
}
