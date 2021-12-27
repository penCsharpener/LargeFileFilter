using LargeFileFilter.Core.Models;

namespace LargeFileFilter.Core.Services.Evaluators;

public class ExcludeEvaluator : Evaluator
{
    private readonly ExcludeFilter _filter;

    public ExcludeEvaluator(ExcludeFilter filter)
    {
        _filter = filter;
    }

    public override bool IncludeLine(string line)
    {
        if (_filter == null)
        {
            return true;
        }

        if (_filter.Equals?.Length > 0)
        {
            return !_filter.Equals.Any(filter => filter.Equals(line));
        }

        var includeLine = true;

        if (_filter.StartsWith?.Length > 0)
        {
            includeLine = !_filter.StartsWith.Any(line.StartsWith);
        }

        if (_filter.EndsWith?.Length > 0)
        {
            includeLine = !_filter.EndsWith.Any(line.EndsWith);
        }

        if (_filter.Contains?.Length > 0)
        {
            includeLine = !_filter.Contains.Any(line.Contains);
        }

        return includeLine;
    }
}