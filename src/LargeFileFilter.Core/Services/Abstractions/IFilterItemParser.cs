using LargeFileFilter.Core.Models;

namespace LargeFileFilter.Core.Services.Abstractions;

public interface IFilterItemParser
{
    List<FilterItem> ParseFilterItems();
}
