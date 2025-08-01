using BookShob.Application.Interfaces;
using BookShob.Infrastructure;
using BookShob.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Asp.Versioning;
using Asp.Versioning.Conventions;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using ApiVersion = Asp.Versioning.ApiVersion; // ✅ Alias to avoid ambiguity

var builder = WebApplication.CreateBuilder(args);

// ---------- Database ----------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Constr")));

// ---------- AutoMapper ----------
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ---------- Dependency Injection ----------
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
    }
}
