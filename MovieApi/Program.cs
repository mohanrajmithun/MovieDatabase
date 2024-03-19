using MovieDatabase.core;
using MovieDatabase.Application;
using MovieDatabase.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Context;
using System.Security.Claims;
using Newtonsoft.Json;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();
try
{
    Log.Information("Starting web host");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context,services,configuration) => configuration
           .ReadFrom.Configuration(context.Configuration)
           .ReadFrom.Services(services)
           .Enrich.FromLogContext());

    var validIssuer = builder.Configuration.GetValue<string>("JwtTokenSettings:ValidIssuer");
    var validAudience = builder.Configuration.GetValue<string>("JwtTokenSettings:ValidAudience");
    var symmetricSecurityKey = builder.Configuration.GetValue<string>("JwtTokenSettings:SymmetricSecurityKey");
    // Add services to the container.

    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        // Add other options as needed
    });

    var configBuilder = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Load appsettings.json
        .AddEnvironmentVariables();
    builder.Services.AddSingleton(configBuilder);



    builder.Services.AddEndpointsApiExplorer();



    builder.Services.AddControllers().AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddProblemDetails();
    builder.Services.AddApiVersioning(options => { options.AssumeDefaultVersionWhenUnspecified = true; });
    builder.Services.AddRouting(options => options.LowercaseUrls = true);

    builder.Services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Test API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
    });

    builder.Services
        .AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer(options =>
        {
            //options.Authority = "validIssuer";
            //options.RequireHttpsMetadata = false;
            options.IncludeErrorDetails = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = validIssuer,
                ValidAudience = validAudience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(symmetricSecurityKey)
                ),
            };
        });

    builder.Services.AddScoped<IMovieRepository, MovieRepository>();
    builder.Services.AddScoped<IMovieService, MovieService>();
    builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

    builder.Services.AddScoped<TokenService, TokenService>();


    var app = builder.Build();

    app.UseSerilogRequestLogging(configure =>
    {
        configure.EnrichDiagnosticContext = async (diagnosticContext, httpContext) =>
        {
            // Get the user ID from the current user's claims
            var userId = httpContext.User?.FindFirst(ClaimTypes.Name)?.Value;

            // Add the user ID to the diagnostic context
            diagnosticContext.Set("UserId", userId);
        };
        configure.MessageTemplate = "HTTP {RequestMethod} {RequestPath} ({UserId}) responded {StatusCode} in {Elapsed:0.0000}ms";
    }); // We want to log all HTTP requests

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();
    }



    app.UseHttpsRedirection();

    app.UseAuthentication();

  
    app.UseAuthorization();

    app.MapControllers();

    //app.MapGet("/api/users/login", (IDiagnosticContext diagnosticContext, HttpContext httpContext) =>
    //{
    //    // You can enrich the diagnostic context with custom properties.
    //    // They will be logged with the HTTP request.
    //    diagnosticContext.Set("UserId", "someone");
    //});

    app.Run();

}

catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;

}
finally
{
    Log.CloseAndFlush();
}

return 0;
