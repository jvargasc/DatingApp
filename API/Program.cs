using API.Data;
using API.Entities;
using API.Extensions;
using API.MiddleWare;
using API.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    ///
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(builder =>
    builder.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins("https://localhost:4200")
    );

app.UseAuthentication(); //Do you have a valid token?
app.UseAuthorization(); //What are the scopes of your actions

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");
app.MapFallbackToController("Index", "Fallback");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await Seed.ClearConnections(context);
    await Seed.SeedUsers(userManager, roleManager);
}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();