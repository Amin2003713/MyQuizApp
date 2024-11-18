using Microsoft.EntityFrameworkCore;

namespace MyQuizApp.WebApi.Data;

public static class Extensions
{
    public static async void ConfigureAutomaticMigrations( this WebApplication app)
    {
        
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<Context>();
        if (db == null)
            return;

        await db.Database.MigrateAsync();


        if ((await db.Database.GetPendingMigrationsAsync()).Any())
            await db.Database.MigrateAsync();
        
    }
}