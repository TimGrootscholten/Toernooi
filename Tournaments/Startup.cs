using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Migrations;
using Services;
using Tournaments.Helpers;

namespace Tournaments;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; set; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options => options.AddDefaultPolicy(
            builder =>
            {
                builder.WithOrigins("http://localhost:3000");
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            }));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "tournaments.nl",
                    ValidAudience = "tournaments.nl",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("tournaments@3102")),
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddSwaggerGen(s =>
        {
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter JWT Bearer token **_only_**",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer", // must be lower case
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            s.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            s.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {securityScheme, new string[] { }}
            });
            s.SwaggerDoc("v1", new OpenApiInfo {Title = "Tournament", Version = "v1"});
            s.CustomOperationIds(e => e.ActionDescriptor.RouteValues["action"]);
            s.DocumentFilter<SwaggerFilters>();
        });

        services.AddDbContext<TournamentDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("TournamentDb")));
        services.DependencyInjection(Configuration);

        services.AddControllers();
        services.AddHttpContextAccessor();

        // Disable BuildServiceProvider warning, no other options to resolve
        #pragma warning disable ASP0000
        var databaseService = services.BuildServiceProvider().GetService<IDatabaseService>();
        databaseService?.CheckDatabaseConnection();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tournament v1"));
        }

        app.UseHttpsRedirection();

        app.UseCors();
        app.UseRouting();
        app.UseAuthorization();
        app.UseAuthentication();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}