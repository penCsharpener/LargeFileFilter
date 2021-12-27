using System.IO.Abstractions;
using LargeFileFilter.Core.Models.Settings;
using LargeFileFilter.Core.Services.Abstractions;

namespace LargeFileFilter.Core.Services
{
    public class FileFilterService : IFileFilterService
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILineEvaluator _lineEvaluator;
        private readonly AppSettings _appSettings;

        public FileFilterService(IFileSystem fileSystem, ILineEvaluator lineEvaluator, AppSettings appSettings)
        {
            _fileSystem = fileSystem;
            _lineEvaluator = lineEvaluator;
            _appSettings = appSettings;
        }

        public async Task FilterFileAsync(string pathToFile, CancellationToken cancellationToken = default)
        {
            if (!_fileSystem.File.Exists(pathToFile))
            {
                return;
            }

            var fileInfo = new FileInfo(pathToFile);
            var newFilePath = _fileSystem.Path.Combine(fileInfo.DirectoryName, _fileSystem.Path.GetFileNameWithoutExtension(fileInfo.Name) + "_filtered" + _fileSystem.Path.GetExtension(pathToFile));
            await _fileSystem.File.WriteAllTextAsync(newFilePath, string.Empty, cancellationToken);

            using (var reader = _fileSystem.File.OpenText(pathToFile))
            {
                var line = string.Empty;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (_lineEvaluator.IncludeLine(line))
                    {
                        await _fileSystem.File.AppendAllTextAsync(newFilePath, line + Environment.NewLine, cancellationToken);
                    }
                }
            }
        }
    }
}
