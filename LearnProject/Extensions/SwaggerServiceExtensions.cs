using LearnProject.Dtos.request;
using LearnProject.DTOs.request;
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
            });

            // ลงทะเบียน Example Filters จาก Assembly
            services.AddSwaggerExamplesFromAssemblyOf<RegisterRequestExample>();
            services.AddSwaggerExamplesFromAssemblyOf<LoginRequestExample>();

            return services;

        }


    }
}
