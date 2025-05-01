using Homework1.Data;
using Homewrok1;
using Homewrok1.Options;

namespace Homework1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<ReqResOptions>(
                builder.Configuration.GetSection("ReqRes"));

            builder.Services.Configure<JsonPlaceholderOptions>(
                builder.Configuration.GetSection("JsonPlaceholder"));

            builder.Services.AddHttpClient<ReqResClient>();
            builder.Services.AddHttpClient<JsonPlaceholderClient>();
            // Add controllers
            builder.Services.AddControllers();

            // Add Swagger for API documentation
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Use HTTPS redirection and authorization
            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Map controllers
            app.MapControllers();

            // Run the application
            app.Run();
        }
    }
}
