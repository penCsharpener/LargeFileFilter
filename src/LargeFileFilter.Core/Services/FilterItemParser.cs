using LargeFileFilter.Core.Models;
using LargeFileFilter.Core.Models.Enums;
using LargeFileFilter.Core.Models.Settings;
using LargeFileFilter.Core.Services.Abstractions;

namespace LargeFileFilter.Core.Services
{
    public class FilterItemParser : IFilterItemParser
    {
        private readonly AppSettings _appSettings;

        private static readonly Dictionary<char, int> _filterSwitchMapping = new()
        {
            { 'r', (int)FilterSwitches.Regex },
            { 's', (int)FilterSwitches.StartsWith },
            { '<', (int)FilterSwitches.StartsWith },
            { 'e', (int)FilterSwitches.EndsWith },
            { '>', (int)FilterSwitches.EndsWith },
            { 'n', (int)FilterSwitches.Not },
            { '!', (int)FilterSwitches.Not },
            { 'c', (int)FilterSwitches.Contains },
            { '=', (int)FilterSwitches.EqualMatch },
            { 'm', (int)FilterSwitches.EqualMatch },
            { 'x', (int)FilterSwitches.Exclude },
        };

        public FilterItemParser(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public List<FilterItem> ParseFilterItems()
        {
            var filterItems = new List<FilterItem>();

            foreach (var item in _appSettings.FilterItems)
            {
                var filterSwitch = ParseSwitches(item.Switches);
                var filterSegment = GetFilterSegmentType(filterSwitch);

                filterItems.Add(new(item.FilterText, filterSwitch, filterSegment));
            }

            return filterItems;
        }

        private FilterSegments GetFilterSegmentType(FilterSwitches filterSwitches)
        {
            return filterSwitches switch
            {
                FilterSwitches.Not | FilterSwitches.EqualMatch => FilterSegments.EqualsNot,
                FilterSwitches.EqualMatch => FilterSegments.Equals,
                FilterSwitches.Not | FilterSwitches.StartsWith => FilterSegments.StartsNotWith,
                FilterSwitches.StartsWith => FilterSegments.StartsWith,
                FilterSwitches.Not | FilterSwitches.EndsWith => FilterSegments.EndsNotWith,
                FilterSwitches.EndsWith => FilterSegments.EndsWith,
                FilterSwitches.Not | FilterSwitches.Contains => FilterSegments.ContainsNot,
                _ => FilterSegments.Contains
            };
        }

        private FilterSwitches ParseSwitches(string switchesRaw)
        {
            if (switchesRaw.Length > 0)
            {
                var switchSum = (FilterSwitches)switchesRaw.ToCharArray().Sum(s => _filterSwitchMapping[s]);

                if (switchSum is (FilterSwitches.StartsWith | FilterSwitches.Regex) ||
                    switchSum is (FilterSwitches.StartsWith | FilterSwitches.Contains) ||
                    switchSum is (FilterSwitches.StartsWith | FilterSwitches.EqualMatch) ||
                    switchSum is (FilterSwitches.StartsWith | FilterSwitches.EndsWith) ||
                    switchSum is (FilterSwitches.EndsWith | FilterSwitches.Regex) ||
                    switchSum is (FilterSwitches.EndsWith | FilterSwitches.Contains) ||
                    switchSum is (FilterSwitches.EndsWith | FilterSwitches.EqualMatch) ||
                    switchSum is (FilterSwitches.Contains | FilterSwitches.Regex) ||
                    switchSum is (FilterSwitches.Contains | FilterSwitches.EqualMatch) ||
                    switchSum is (FilterSwitches.Regex | FilterSwitches.EqualMatch))
                {
                    throw new Exception($"invalid combination of switches: '{switchesRaw}'");
                }

                return switchSum;
            }

            return FilterSwitches.None;
        }
    }
}
