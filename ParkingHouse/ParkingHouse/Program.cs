
using Microsoft.OpenApi.Models;
using ParkingHouse.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// AddSingleton DI keeps ParkingService's Dictionary in Memory
builder.Services.AddSingleton<IParkingService, ParkingService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
// Using Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
