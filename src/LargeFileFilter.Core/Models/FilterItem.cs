namespace LargeFileFilter.Core.Models;

public class FilterItem
{
    public string FilterText { get; set; }
    public string RegexPattern { get; set; }
    public bool IsRegex { get; set; }
    public FilterSegments FilterSegment { get; set; }
}