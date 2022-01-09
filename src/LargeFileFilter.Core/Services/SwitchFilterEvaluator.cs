using LargeFileFilter.Core.Models;
using LargeFileFilter.Core.Models.Enums;
using LargeFileFilter.Core.Services.Abstractions;
using System.Text.RegularExpressions;

namespace LargeFileFilter.Core.Services;

public class SwitchFilterEvaluator : ILineEvaluator
{
    private readonly IFilterItemParser _filterItemParser;

    public SwitchFilterEvaluator(IFilterItemParser filterItemParser)
    {
        _filterItemParser = filterItemParser;
    }

    public bool IncludeLine(string line)
    {
        var filterItems = _filterItemParser.ParseFilterItems();
        var excludeFilterItems = filterItems.Where(f => f.FilterSwitches.HasFlag(FilterSwitches.Exclude)).ToList();
        var includeFilterItems = filterItems.Where(f => !f.FilterSwitches.HasFlag(FilterSwitches.Exclude)).ToList();
        var includeLine = excludeFilterItems.Count > 0 && includeFilterItems.Count == 0;

        foreach (var item in excludeFilterItems)
        {
            if (EvaluateFilter(line, item))
            {
                return false;
            }
        }

        foreach (var item in includeFilterItems)
        {
            includeLine = includeLine || EvaluateFilter(line, item);
        }

        //for (int i = 0; i < filterItems.Count; i++)
        //{
        //    includeLine = includeLine || EvaluateFilter(line, filterItems[i]);
        //}

        return includeLine;
    }

    private bool EvaluateFilter(string line, FilterItem filterItem)
    {
        var includeLine = false;

        if (filterItem.FilterSwitches.HasFlag(FilterSwitches.Regex))
        {
            var regex = new Regex(filterItem.FilterText).Match(line).Length > 0;

            includeLine = filterItem.FilterSwitches.HasFlag(FilterSwitches.Not) ? !regex : regex;
        }

        if (filterItem.FilterSwitches.HasFlag(FilterSwitches.StartsWith))
        {
            var startsWith = line.StartsWith(filterItem.FilterText);

            includeLine = filterItem.FilterSwitches.HasFlag(FilterSwitches.Not) ? !startsWith : startsWith;
        }

        if (filterItem.FilterSwitches.HasFlag(FilterSwitches.EndsWith))
        {
            var endsWith = line.EndsWith(filterItem.FilterText);

            includeLine = filterItem.FilterSwitches.HasFlag(FilterSwitches.Not) ? !endsWith : endsWith;
        }

        if (filterItem.FilterSwitches.HasFlag(FilterSwitches.Contains))
        {
            var contains = line.Contains(filterItem.FilterText);

            includeLine = filterItem.FilterSwitches.HasFlag(FilterSwitches.Not) ? !contains : contains;
        }

        if (filterItem.FilterSwitches.HasFlag(FilterSwitches.EqualMatch))
        {
            var equals = line.Equals(filterItem.FilterText);

            includeLine = filterItem.FilterSwitches.HasFlag(FilterSwitches.Not) ? !equals : equals;
        }

        return includeLine;
    }
}
