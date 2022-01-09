using LargeFileFilter.Core.Models.Enums;

namespace LargeFileFilter.Core.Models;

public class FilterItem
{
    public FilterItem(string filterText, FilterSwitches filterSwitches, FilterSegments filterSegment)
    {
        FilterText = filterText;
        FilterSwitches = filterSwitches;
        FilterSegment = filterSegment;
    }

    public string FilterText { get; set; }
    public FilterSwitches FilterSwitches { get; set; }
    public FilterSegments FilterSegment { get; set; }
}