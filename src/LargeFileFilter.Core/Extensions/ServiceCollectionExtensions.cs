using LargeFileFilter.Core.Models.Settings;
using LargeFileFilter.Core.Services;
using LargeFileFilter.Core.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Abstractions;

namespace LargeFileFilter.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLargeFileFilterServices(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

            services.AddSingleton(appSettings);
            services.AddScoped<IFileFilterService, FileFilterService>();
            services.AddScoped<IFilterItemParser, FilterItemParser>();
            services.AddScoped<IFileSystem, FileSystem>();
            services.AddScoped<ILineEvaluator, SwitchFilterEvaluator>();
            services.AddScoped<IEvaluatorFactory, EvaluatorFactory>();

            return services;
        }
    }
}
