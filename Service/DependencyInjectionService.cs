using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Migrations;
using Repositories;
using Services.User;

namespace Services
{
    public static class DependencyInjectionService
    {
        public static void DependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);
            
            services.AddSingleton<IApiExceptionService, ApiExceptionService>();
            services.AddTransient<IDatabaseService, DatabaseService>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();

            services.AddTransient<ITeamService, TeamService>();
            services.AddTransient<ITeamRepository, TeamRepository>();

            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ITokenRepository, TokenRepository>();
        }
    }
}
