using LearnProject.Dtos.request;
using LearnProject.DTOs.request;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace LearnProject.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations(); // เปิดการใช้งาน Swagger Annotations
                c.ExampleFilters();    // เปิดใช้งาน Example Filters

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer",
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            // ลงทะเบียน Example Filters จาก Assembly
            services.AddSwaggerExamplesFromAssemblyOf<RegisterRequestExample>();
            services.AddSwaggerExamplesFromAssemblyOf<LoginRequestExample>();

            return services;
        }
    }
}
