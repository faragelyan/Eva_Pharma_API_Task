using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Conventions;
using BookShob.Application.Interfaces;
using BookShob.Infrastructure;
using BookShob.Infrastructure.Repositories;
using BookShob.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity; // ✅ Alias to avoid ambiguity
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using ApiVersion = Asp.Versioning.ApiVersion;

var builder = WebApplication.CreateBuilder(args);

// ---------- Database ----------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Constr")));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add Authentication (JWT)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],      // e.g. "https://localhost:5001"
        ValidAudience = builder.Configuration["Jwt:Audience"],  // e.g. "https://localhost:5001"
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

// Add Authorization
builder.Services.AddAuthorization();

// ---------- AutoMapper ----------
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ---------- Dependency Injection ----------
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ---------- Response Caching ----------
builder.Services.AddResponseCaching();

// ---------- MVC + Cache Profiles ----------
builder.Services.AddControllersWithViews(options =>
{
    options.CacheProfiles.Add("ShortTerm", new CacheProfile
    {
        Duration = 15,
        Location = ResponseCacheLocation.Any,
        NoStore = false
    });

    options.CacheProfiles.Add("LongTerm", new CacheProfile
    {
        Duration = 120,
        Location = ResponseCacheLocation.Client,
        NoStore = false
    });
});

// ---------- API Versioning ----------
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0); // ✅ Using alias to avoid ambiguity
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddMvc(options =>
{
    options.Conventions.Add(new VersionByNamespaceConvention());
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // Formats: v1, v2
    options.SubstituteApiVersionInUrl = true;
});

// ---------- Swagger ----------
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();


var app = builder.Build();

// ---------- Middleware ----------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "BookShob API V1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "BookShob API V2");
    });
}

app.UseHttpsRedirection();
app.UseResponseCaching();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

// ---------- Swagger Config Class ----------
public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        this.provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Title = $"BookShob API {description.ApiVersion}",
                Version = description.ApiVersion.ToString()
            });
        }

        // ✅ Add JWT bearer auth
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter 'Bearer' [space] and then your valid token.\nExample: \"Bearer eyJhbGci...\""
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    }

}
