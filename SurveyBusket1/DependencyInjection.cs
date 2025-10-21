using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SurveyBusket8.Persistence;
using SurveyBusket8.Services;
using System.Reflection;

namespace SurveyBusket8;

public static class DependencyInjection
{
   
    public static IServiceCollection AddDependencies(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddControllers();
        var ConnectionString = configuration.GetConnectionString("DefaultConnections") ??
    throw new InvalidOperationException("connection string 'Default Connection' not found");

        //Authentication
        services.AddAuthConfig(configuration);

        services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(ConnectionString));
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPollService, PollService>();

        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton<IMapper>(new Mapper(mappingConfig));
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();


        services.AddFluentValidationAutoValidation()
         .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddHttpContextAccessor();

        return services;
    }
    public static IServiceCollection AddAuthConfig(this IServiceCollection services , IConfiguration configuration)
    {
        services.AddIdentityApiEndpoints<ApplicationUser>()
            .AddEntityFrameworkStores<ApplicationDBContext>();
        var settings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

        //services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
            
        services.AddSingleton<IJwtProvider, JwtProvider>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options => {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey= true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime= true,
                    ValidAudience = settings.Audience!,
                    ValidIssuer = settings.Issuer!,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(settings.key!))
                  
                };
            });
        return services;
    }
}
