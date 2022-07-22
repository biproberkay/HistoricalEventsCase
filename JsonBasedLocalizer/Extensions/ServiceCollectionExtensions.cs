using Microsoft.AspNetCore.Localization;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Reflection;

namespace JsonBasedLocalizer.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static void AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "JsonBasedLocalization",
                    Version = "v1",
                    Description = "An .NET Web API for researching Historical Events",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Şükrü Berkay",
                        Url = new Uri("https://github.com/biproberkay")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://example.com/license")
                    }
                });
            });
        }

        internal static IServiceCollection AddCustomLocalization(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            return services;
        }
    }
}
