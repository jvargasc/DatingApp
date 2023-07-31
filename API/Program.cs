using System.Text;
using API.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// Commented in the course
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //Commented in the course
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

//Commented in the course
// app.UseHttpsRedirection();

app.UseCors(builder =>
    builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200")
    );

app.UseAuthentication(); //Do you have a valid token?
app.UseAuthorization(); //What are the scopes of your actions

app.MapControllers();

app.Run();