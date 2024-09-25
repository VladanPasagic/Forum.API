using Forum.Core.Repositories;
using Forum.Core.Repositories.Interfaces;
using Forum.Core.Services;
using Forum.Core.Services.Interfaces;
using Forum.EF;
using Forum.EF.Entities;
using Forum.SIEM.Core.Repositories;
using Forum.SIEM.Core.Repositories.Interfaces;
using Forum.SIEM.Core.Services;
using Forum.SIEM.Core.Services.Interfaces;
using Forum.SIEM.EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Core;
using System.Text;

namespace Forum.API.DependancyInjection;

public static class DIContainer
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration _configuration)
    {
        #region CORS
        services.AddCors(options =>
        {
            options.AddPolicy(name: "MyAllowSpecificOrigin", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        });
        #endregion
        #region ASP.NET Identity
        services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ForumContext>().AddDefaultTokenProviders();
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 4;
        });
        #endregion
        #region JWT
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme =
            x.DefaultChallengeScheme =
            x.DefaultScheme =
            x.DefaultForbidScheme =
            x.DefaultSignInScheme =
            x.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!);
            o.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["JWT:Issuer"],
                ValidAudience = _configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero,
            };
            o.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync("{\"error\": \"Unauthorized\"}");
                },
                OnForbidden = context =>
                {
                    context.Response.StatusCode = 403;
                    return Task.CompletedTask;
                }
            };
        });
        services.AddAuthorization();
        #endregion
        #region Automapper
        services.AddAutoMapper(typeof(Forum.Core.Mapper.AutoMapperConfiguration));
        services.AddAutoMapper(typeof(SIEM.Core.Mapper.AutoMapperConfiguration));
        #endregion
        #region Database
        services.AddDbContext<ForumContext>();
        services.AddDbContext<SiemContext>();
        #endregion
        #region Logger
        Logger logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.Console().WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Hour).CreateLogger();
        services.AddSerilog(logger);
        #endregion
        #region Repositories
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<ILogEntryRepository, LogEntryRepository>();
        #endregion
        #region Services
        services.AddScoped<IJWTManagerService, JWTManagerService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddSingleton<IEmailSender<User>, EmailService>();
        services.AddScoped<IRandomCodeGeneratorService, RandomCodeGeneratorService>();
        services.AddScoped<IPolicyCheckService, PolicyCheckService>();
        services.AddScoped<ILogEntryService, LogEntryService>();
        #endregion
        #region Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        #endregion
        #region Controllers
        services.AddControllers();
        #endregion
        return services;
    }
}
