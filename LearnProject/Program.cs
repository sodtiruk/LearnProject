using LearnProject.Data;
using LearnProject.Extensions;
using LearnProject.Repositories;
using LearnProject.Repositories.impl;
using LearnProject.Services;
using LearnProject.Utils;
using Microsoft.EntityFrameworkCore;

namespace LearnProject
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ECommerceDbContext>(options =>
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

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}