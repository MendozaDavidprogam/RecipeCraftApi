using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RecipeCraftApi.Infrastructure.Database.Context;
using DotNetEnv;

namespace RecipeCraftApi.Infrastructure.Database.Context
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Cargar .env
            Env.Load();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            var connectionString = $"Host={Env.GetString("DB_HOST")};" +
                                   $"Port={Env.GetString("DB_PORT")};" +
                                   $"Database={Env.GetString("DB_NAME")};" +
                                   $"Username={Env.GetString("DB_USER")};" +
                                   $"Password={Env.GetString("DB_PASSWORD")}";

            optionsBuilder.UseNpgsql(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
