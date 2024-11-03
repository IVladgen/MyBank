using Microsoft.OpenApi.Models;
using System.Reflection;

namespace MyBank.API.Swagger
{
    public static class SwaggerStartup
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "ExceleTech.API",
                    Description = "Documentation MyBank",
                    TermsOfService = new Uri("https://t.me/VLDSLV"),
                    Contact = new OpenApiContact()
                    {
                        Name = "Lead",
                        Url = new Uri("https://t.me/VLDSLV")
                    },

                });

                var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
                options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                   {
                       new OpenApiSecurityScheme
                       {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                       },
                       new string[] {}
                   }
                });
            });
        }
    }
}
