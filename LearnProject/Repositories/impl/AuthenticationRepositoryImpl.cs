using LearnProject.Data;
using LearnProject.Dtos.request;
using LearnProject.DTOs.request;
using LearnProject.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnProject.Repositories.impl
{
    public class AuthenticationRepositoryImpl : IAuthenticationRepository
    {

        private readonly AppDbContext dbContext;

        public AuthenticationRepositoryImpl(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<UserModel?> GetUserByUsername(RegisterRequest registerRequest)
        {
            return await dbContext.Users.FirstOrDefaultAsync(user => user.Email == registerRequest.Email);
        }
        public async Task<UserModel?> GetUserByUsername(LoginRequest loginRequest)
        {
            return await dbContext.Users.FirstOrDefaultAsync(user => user.Email == loginRequest.Email);
        }

        public async Task<UserModel> AddUser(UserModel newUser)
        {
            dbContext.Users.Add(newUser);
            await dbContext.SaveChangesAsync();
            return newUser; 
        }


    }
}
