using Microsoft.EntityFrameworkCore;
using MinimalTranslator.Database;

namespace MinimalTranslator.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async void ApplyMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();

        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            await context.Database.MigrateAsync();
        }
        catch(Exception ex)
        {
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(ex, "Error in migration");
        }
    }

    // public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    // {
    //     app.UseMiddleware<ExceptionHandlingMiddleware>();
    // }
}