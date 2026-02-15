using Microsoft.AspNetCore.Localization;
using Home_Expert.Resources;
using System.Globalization;

namespace Home_Expert.DependencyInjections
{
    public static class LocalizationDependencyInjection
    {
        public static IServiceCollection AddLocalizationDependencyInjection(this IServiceCollection services)
        {
            #region Localization
            services.AddControllersWithViews()
                            .AddViewLocalization()
                            .AddDataAnnotationsLocalization(options =>
                            {
                                options.DataAnnotationLocalizerProvider=(type, factory) =>
                                factory.Create(typeof(SharedResource));
                            });
            services.AddLocalization(opt =>
            {
                opt.ResourcesPath = "";
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                List<CultureInfo> supportedCultures = new List<CultureInfo>
        {
            new CultureInfo("en-US"),
            new CultureInfo("ar-JO")
        };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            #endregion
            return services;
        }
    }
}
