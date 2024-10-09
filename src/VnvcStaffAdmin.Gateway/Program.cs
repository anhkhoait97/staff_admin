using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using VnvcStaffAdmin.Gateway.Config;
using VnvcStaffAdmin.Identity.ConfigurationServices;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var routesFolder = $"Routes/{builder.Environment.EnvironmentName}";

builder.Configuration.AddOcelotWithSwaggerSupport(options =>
{
    options.Folder = routesFolder;
});

builder.Services.AddOcelot(builder.Configuration)
    .AddCacheManager(options => { options.WithDictionaryHandle(); })
    .AddPolly();
builder.Services.AddSwaggerForOcelot(builder.Configuration);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddOcelot(routesFolder, builder.Environment)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddJWTAuthen(configuration);
builder.Services.AddCustomAuthorize();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("CorsPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseSwaggerForOcelotUI(options =>
{
    options.PathToSwaggerGenerator = "/swagger/docs";
    options.ReConfigureUpstreamSwaggerJson = AlterUpstream.AlterUpstreamSwaggerJson;

}).UseOcelot().Wait();

app.MapControllers();

app.Run();
