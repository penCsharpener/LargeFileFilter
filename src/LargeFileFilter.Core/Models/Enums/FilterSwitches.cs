namespace LargeFileFilter.Core.Models.Enums;

[Flags]
public enum FilterSwitches
{
    None = 1,
    Regex = 2,
    Contains = 4,
    StartsWith = 8,
    EndsWith = 16,
    EqualMatch = 32,
    Not = 64,
    Exclude = 128,
}
