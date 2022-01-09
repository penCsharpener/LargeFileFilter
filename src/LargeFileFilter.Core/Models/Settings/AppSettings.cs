namespace LargeFileFilter.Core.Models.Settings
{
    public class AppSettings
    {
        public string FilePath { get; set; }
        public IncludeFilter IncludeFilters { get; set; }
        public ExcludeFilter ExcludeFilters { get; set; }
        public FilterItemRaw[] FilterItems { get; set; }
    }
}
