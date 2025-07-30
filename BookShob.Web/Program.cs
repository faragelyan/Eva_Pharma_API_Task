using BookShob.Infrastructure;
using Microsoft.EntityFrameworkCore;
using BookShob.Application.Mapping;
using Microsoft.Extensions.DependencyInjection;
using BookShob.Application.Interfaces;
using BookShob.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Constr")));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//builder.Services.AddAutoMapper(typeof(ProductProfile), typeof(CategoryProfile));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

/*builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);
builder.Services.AddAutoMapper(typeof(CategoryProfile).Assembly);*/
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API V1", Version = "v1" });
    options.SwaggerDoc("v2", new OpenApiInfo { Title = "My API V2", Version = "v2" });
});

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

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

builder.Services.AddResponseCaching();

var app = builder.Build();

app.UseResponseCaching(); 


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        options.SwaggerEndpoint("/openapi/v1.json", "api");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
