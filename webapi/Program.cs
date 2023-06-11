using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using webapi.DBConextion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEntityFrameworkMySQL().AddDbContext<DbtestContext>(
    options =>
    {
        //var app = builder.Configuration.GetConnectionString("DefaultConnection");
        options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
