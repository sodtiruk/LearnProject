using System.Text;
using LearnProject.Data;
using LearnProject.Extensions;
using LearnProject.Repositories;
using LearnProject.Repositories.impl;
using LearnProject.Services;
using LearnProject.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LearnProject
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("ticketdb");
                var serverVersion = ServerVersion.AutoDetect(connectionString);
                options.UseMySql(connectionString, serverVersion);
            });

            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<AuthenticationService>();

            builder.Services.AddScoped<IProductRepository, ProductRepositoryImpl>();
            builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepositoryImpl>();

            builder.Services.AddSingleton<JwtUtil>();

            // Add authentication services
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var secretKey = builder.Configuration["JwtSettings:SecretKey"];
                    if (string.IsNullOrEmpty(secretKey))
                    {
                        throw new InvalidOperationException("JWT Secret Key is not configured.");
                    }

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                        ClockSkew = TimeSpan.Zero // ลดเวลาให้ตรวจสอบแบบเข้มงวด
                    };
                });


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerService();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}