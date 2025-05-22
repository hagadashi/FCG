using FCG.Domain.Interfaces.Repositories.Games;
using FCG.Domain.Interfaces.Repositories.Users;
using FCG.Infrastructure.Data;
using FCG.Infrastructure.Data.Repositories.Games;
using FCG.Infrastructure.Data.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FCG.Infrastructure
{
    public static class InfrastructureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found");

            // Configuração do DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    connectionString,
                    npgsqlOptions =>
                    {
                        npgsqlOptions.EnableRetryOnFailure();
                        npgsqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                    })
            );

            // Registro dos repositórios
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ILibraryRepository, LibraryRepository>();
            services.AddScoped<ISaleRepository, SaleRepository>();

            // Health Checks
            services.AddHealthChecks()
                .AddDbContextCheck<AppDbContext>()
                .AddNpgSql(connectionString);

            return services;
        }
    }
}