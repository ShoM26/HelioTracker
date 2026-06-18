using Heliotracker;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString =  builder.Configuration.GetConnectionString("DefaultConnection");

var version = new MySqlServerVersion(new Version(8,0,36));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, version));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.Run();