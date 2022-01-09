namespace LargeFileFilter.Core.Models;

public class IncludeFilter : BaseFilter
{
    public string[] StartsWith { get; set; }
    public string[] EndsWith { get; set; }
    public string[] Contains { get; set; }
    public string[] Equals { get; set; }
}
