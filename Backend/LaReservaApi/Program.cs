using LaReservaBackend.Application.DependencyInjection;
using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Infrastructure.Data;
using LaReservaBackend.Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
     .UseSnakeCaseNamingConvention());

builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

// Registrar automáticamente todos los servicios de Application (MediatR handlers)
builder.Services.AddApplication();

// Registrar automáticamente todos los repositorios de Infrastructure
builder.Services.AddInfrastructure();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
 app.UseSwagger();
 app.UseSwaggerUI();
}

app.UseCors(builder =>
    builder.WithOrigins("http://localhost:5173")
           .AllowAnyHeader()
           .AllowAnyMethod()
);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
