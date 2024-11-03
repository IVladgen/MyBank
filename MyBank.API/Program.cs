using MyBank.Infrastructure.DependensyInjection;
using MyBank.Application.DependensyInjection;
using MyBank.Domain.Options;
using MyBank.API.Settings;
using MyBank.API.Swagger;
using Serilog;
using MyBank.API.Middlewares;
using MyBank.API.HostedServices;
var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<TokenOptions>(builder.Configuration.GetSection("TokenOptions"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();
builder.Services.AddJwtSettings(builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>());
builder.Services.AddSwagger();
builder.Services.AddHostedService<CurrencyHostedService>();
builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:5174")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials(); 
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


