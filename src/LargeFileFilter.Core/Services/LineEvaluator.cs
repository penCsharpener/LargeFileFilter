using LargeFileFilter.Core.Models.Settings;
using LargeFileFilter.Core.Services.Abstractions;

namespace LargeFileFilter.Core.Services;

public class LineEvaluator : ILineEvaluator
{
    private readonly IEvaluatorFactory _evaluatorFactory;
    private readonly AppSettings _settings;

    public LineEvaluator(IEvaluatorFactory evaluatorFactory, AppSettings settings)
    {
        _evaluatorFactory = evaluatorFactory;
        _settings = settings;
    }

    public bool IncludeLine(string line)
    {
        var includeFilter = _evaluatorFactory.GetFilter(_settings.IncludeFilters);

        var includeLine = includeFilter?.IncludeLine(line) == true;

        var excludeFilter = _evaluatorFactory.GetFilter(_settings.ExcludeFilters);

        includeLine = includeLine || excludeFilter?.IncludeLine(line) == false;

        return includeLine;
    }
}