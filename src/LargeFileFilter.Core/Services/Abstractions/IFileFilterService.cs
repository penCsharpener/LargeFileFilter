namespace LargeFileFilter.Core.Services.Abstractions
{
    public interface IFileFilterService
    {
        Task FilterFileAsync(string pathToFile, CancellationToken cancellationToken = default);
    }
}
