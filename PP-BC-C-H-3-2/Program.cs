using FluentValidation.AspNetCore;
using FluentValidation;
using PP_BC_C_H_3_2.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

using PP_BC_C_H_3_2.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<GetByIdBookValidator>();
// Replace this line:
//////builder.Services.AddScoped<IValidator<Book>, GetByIdBookValidator>();

// With this line:
builder.Services.AddScoped<IValidator<int>, GetByIdBookValidator>();
// Swaggerf
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
});


// Database Connection
//string connectionStringMsSql = configuration.GetConnectionString("MsSqlConnection");
//builder.Services.AddDbContext<MsSqlDbContext>(options =>
//{
//    options.UseSqlServer(connectionStringMsSql);
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();