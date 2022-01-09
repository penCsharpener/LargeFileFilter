using FluentAssertions;
using LargeFileFilter.Core.Models.Enums;
using LargeFileFilter.Core.Models.Settings;
using LargeFileFilter.Core.Services;
using System;
using Xunit;

namespace LargeFileFilter.IntegrationTests.Services;

public class FilterItemParserTests
{
    private readonly AppSettings _appSettings;
    private readonly FilterItemParser _testObject;

    public FilterItemParserTests()
    {
        _appSettings = new AppSettings();
        _testObject = new FilterItemParser(_appSettings);
    }

    [Theory]
    [InlineData("r", FilterSwitches.Regex)]
    [InlineData("s", FilterSwitches.StartsWith)]
    [InlineData("<", FilterSwitches.StartsWith)]
    [InlineData("e", FilterSwitches.EndsWith)]
    [InlineData(">", FilterSwitches.EndsWith)]
    [InlineData("n", FilterSwitches.Not)]
    [InlineData("!", FilterSwitches.Not)]
    [InlineData("c", FilterSwitches.Contains)]
    [InlineData("!c", FilterSwitches.Contains | FilterSwitches.Not)]
    [InlineData("!e", FilterSwitches.EndsWith | FilterSwitches.Not)]
    [InlineData("=", FilterSwitches.EqualMatch)]
    [InlineData("m", FilterSwitches.EqualMatch)]
    public void FilterItemParser_Works(string switches, FilterSwitches expectedSwitches)
    {
        _appSettings.FilterItems = new FilterItemRaw[] { new() { FilterText = "test", Switches = switches } };

        var filterItem = _testObject.ParseFilterItems()[0];

        filterItem.FilterSwitches = expectedSwitches;
    }

    [Theory]
    [InlineData("sr", FilterSwitches.Regex)]
    [InlineData("sc", FilterSwitches.StartsWith)]
    [InlineData("s=", FilterSwitches.StartsWith | FilterSwitches.Regex)]
    [InlineData("se", FilterSwitches.StartsWith)]
    [InlineData("er", FilterSwitches.EndsWith)]
    [InlineData("ec", FilterSwitches.EndsWith)]
    [InlineData("em", FilterSwitches.Not)]
    [InlineData("cr", FilterSwitches.Not)]
    [InlineData("cm", FilterSwitches.Contains)]
    [InlineData("r=", FilterSwitches.Contains | FilterSwitches.Not)]
    public void FilterItemParser_Throws_Exception(string switches, FilterSwitches expectedSwitches)
    {
        _appSettings.FilterItems = new FilterItemRaw[] { new() { FilterText = "test", Switches = switches } };

        Action action = () => _testObject.ParseFilterItems();

        action.Should().Throw<Exception>();
    }
}
