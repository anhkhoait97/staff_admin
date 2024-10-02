using VnvcStaffAdmin.Identity.ConfigurationServices;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddMongoIdentity();
builder.Services.AddJWTAuthen(builder.Configuration);
builder.Services.AddCustomAuthorize();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authentication API V1");
        c.RoutePrefix = string.Empty;
    });
}


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
