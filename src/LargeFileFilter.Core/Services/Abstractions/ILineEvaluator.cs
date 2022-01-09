namespace LargeFileFilter.Core.Services.Abstractions;

public interface ILineEvaluator
{
    bool IncludeLine(string line);
}
