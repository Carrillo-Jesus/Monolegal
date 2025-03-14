using Microsoft.Extensions.Configuration;
using Monolegal.DataAccess.Persistence;
using Monolegal.Core;
using Monolegal.Core.Interfaces;
using Monolegal.Core.Services;
using Monolegal.DataAccess.Interfaces;
using Monolegal.DataAccess.Repositories;
using Swashbuckle.AspNetCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// Registrar DbContext en la DI container
builder.Services.AddSingleton<DbContext>();

// registrar servicios
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IFacturaService, FacturaService>();
builder.Services.AddScoped<IFacturaRepository, FacturaRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{

    var apiInfo = new OpenApiInfo
    {
        Title = "Facturas Api",
        Description = "APi prueba monolegal, cambiar estados de las facturas",
        Version = "V1",
        Contact = new OpenApiContact
        {
            Name = "Jesus carrillo",
            Email = "jesusdavid4521@gmail.com",

        }

    };

    c.SwaggerDoc("v1", apiInfo);

});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API v1");
    });
}
app.UseCors("AllowAnyOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
