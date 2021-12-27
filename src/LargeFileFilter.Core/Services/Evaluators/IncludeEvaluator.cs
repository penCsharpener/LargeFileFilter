using LargeFileFilter.Core.Models;

namespace LargeFileFilter.Core.Services.Evaluators;

public class IncludeEvaluator : Evaluator
{
    private readonly IncludeFilter _filter;

    public IncludeEvaluator(IncludeFilter filter)
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
            return _filter.Equals.Any(filter => filter.Equals(line));
        }

        var includeLine = false;

        if (_filter.StartsWith?.Length > 0)
        {
            includeLine = includeLine || _filter.StartsWith.Any(line.StartsWith);
        }

        if (_filter.EndsWith?.Length > 0)
        {
            includeLine = includeLine || _filter.EndsWith.Any(line.EndsWith);
        }

        if (_filter.Contains?.Length > 0)
        {
            includeLine = includeLine || _filter.Contains.Any(line.Contains);
        }

        return includeLine;
    }
}