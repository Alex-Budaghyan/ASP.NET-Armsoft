using Homework1.Data;
using Homewrok1;
using Homewrok1.Options;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;

namespace Homework1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();

            builder.Services.Configure<ReqResOptions>(
                builder.Configuration.GetSection("ReqRes"));

            builder.Services.Configure<JsonPlaceholderOptions>(
                builder.Configuration.GetSection("JsonPlaceholder"));

            builder.Services.AddHttpClient<ReqResClient>();
            builder.Services.AddHttpClient<JsonPlaceholderClient>();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;

                    if (exception is InvalidOperationException)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsJsonAsync(new { message = "Validation failed." });
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsJsonAsync(new { message = "An unexpected error occurred." });
                    }
                });
            });

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
