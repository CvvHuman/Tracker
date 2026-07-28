using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tracker.Application.Abstractions;
using Tracker.Infrastructure.Persistence;
using Tracker.Infrastructure.Repository;
using Tracker.Infrastructure.Security;

namespace Tracker.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");// из json

            services.AddDbContext<TrackerDbContext>(options =>
                options.UseNpgsql(connectionString));
            services.AddScoped<ITrackerDbContext>(provider => provider.GetRequiredService<TrackerDbContext>());


            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));//Привязываем секцию JwtSettings из appsettings.json к классу настроек
           
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
