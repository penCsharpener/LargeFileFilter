using LargeFileFilter.Core.Extensions;
using LargeFileFilter.Core.Models.Settings;
using LargeFileFilter.Core.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, false)
    .AddUserSecrets<Program>();

var configuration = configurationBuilder.Build();

var services = new ServiceCollection()
    .AddLargeFileFilterServices(configuration);

var serviceProvider = services.BuildServiceProvider();

var fileFilter = serviceProvider.GetRequiredService<IFileFilterService>();
var settings = serviceProvider.GetRequiredService<AppSettings>();

await fileFilter.FilterFileAsync(settings.FilePath).ConfigureAwait(false);