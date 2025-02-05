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
using Newtonsoft.Json;

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

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = async context =>
                        {
                            if (!context.Response.HasStarted)
                            {
                                // ตรวจสอบว่า Token หมดอายุหรือไม่
                                var errorResponse = context.Exception.GetType() == typeof(SecurityTokenExpiredException)
                                    ? new { error = "Token expired", message = context.Exception.Message }
                                    : new { error = "Invalid token", message = context.Exception.Message };

                                context.Response.StatusCode = 401;
                                context.Response.ContentType = "application/json";
                                await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
                            }

                            // ยกเลิกการประมวลผลเพิ่มเติม
                            context.NoResult();
                        },
                        OnChallenge = async context =>
                        {
                            if (!context.Response.HasStarted)
                            {
                                context.HandleResponse(); // หยุดการตอบสนองอัตโนมัติ

                                context.Response.StatusCode = 401;
                                context.Response.ContentType = "application/json";

                                var errorResponse = new
                                {
                                    error = "Unauthorized",
                                    message = "Authorization token is missing or invalid"
                                };

                                await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
                            }
                        }
                    };
                });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerService();
            builder.Services.AddHealthChecks();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173") // ต้นทางที่อนุญาต
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials();
                    });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowSpecificOrigin");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapHealthChecks("/health");

            app.Run();
        }
    }
}
