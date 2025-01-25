
using LearnProject.Dtos.request;
using LearnProject.DTOs.request;
using LearnProject.Models;

namespace LearnProject.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<UserModel> AddUser(UserModel newUser);
        Task<UserModel?> GetUserByUsername(RegisterRequest registerRequest);
        Task<UserModel?> GetUserByUsername(LoginRequest loginRequest); 
    }
}
