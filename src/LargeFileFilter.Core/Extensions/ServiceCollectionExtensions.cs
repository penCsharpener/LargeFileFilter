using System.IO.Abstractions;
using LargeFileFilter.Core.Models.Settings;
using LargeFileFilter.Core.Services;
using LargeFileFilter.Core.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LargeFileFilter.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLargeFileFilterServices(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

            services.AddSingleton(appSettings);
            services.AddScoped<IFileFilterService, FileFilterService>();
            services.AddScoped<IFileSystem, FileSystem>();
            services.AddScoped<ILineEvaluator, LineEvaluator>();
            services.AddScoped<IEvaluatorFactory, EvaluatorFactory>();

            return services;
        }
    }
}
