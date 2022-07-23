using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace HistoricalEventsCase.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        internal static void ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JsonBasedLocalization v1"));
        }
        internal static IApplicationBuilder UseRequestLocalizationByCulture(this IApplicationBuilder app)
        {
            var supportedCultures = new List<CultureInfo>
            {
                CultureInfo.GetCultureInfo("tr-TR"),
                CultureInfo.GetCultureInfo("it-IT")
            };
            var requestLocalizationOptions = new RequestLocalizationOptions
            {                
                DefaultRequestCulture = new RequestCulture(culture: "tr-TR", uiCulture: "tr-TR"),
                SupportedUICultures = supportedCultures,
                SupportedCultures = supportedCultures,
                RequestCultureProviders = new[] 
                { 
                    new RouteDataRequestCultureProvider { IndexOfCulture = 1, IndexofUICulture = 1 } 
                }
            };
            app.UseRequestLocalization(requestLocalizationOptions);
            return app;
        }

    }
}
