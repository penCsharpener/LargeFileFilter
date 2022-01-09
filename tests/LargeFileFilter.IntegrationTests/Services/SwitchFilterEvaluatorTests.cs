using FluentAssertions;
using LargeFileFilter.Core.Models;
using LargeFileFilter.Core.Models.Enums;
using LargeFileFilter.Core.Models.Settings;
using LargeFileFilter.Core.Services;
using LargeFileFilter.Core.Services.Abstractions;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LargeFileFilter.IntegrationTests.Services;

public class SwitchFilterEvaluatorTests
{
    private const string _sampleLine1 = "The quick brown fox jumps over the lazy dog.";
    private readonly IFilterItemParser _filterParser;
    private readonly SwitchFilterEvaluator _testObject;

    private readonly static string[] _sampleLogLines =
    {
        "2022-01-07 14:48:29.847 +01:00 [INF] Executing some endpoint",
        "2022-01-07 14:48:29.847 +01:00 [DBG] Closing connection to database 'products' on server 'localhost'.",
        "2022-01-07 14:48:29.847 +01:00 [DBG] Closed connection to database 'products' on server 'localhost'.",
        "2022-01-07 14:48:29.847 +01:00 [DBG] Attempting to bind parameter 'productId' of type 'System.Int32' ...",
        "2022-01-07 14:48:29.847 +01:00 [DBG] Done attempting to bind parameter 'productId' of type 'System.Int32'.",
        "2022-01-07 14:48:29.847 +01:00 [DBG] Executing DbCommand [Parameters=[@___id_0='?' (DbType = Int32), @__p_2='?' (DbType = Int32), CommandType='\"Text\"', CommandTimeout='30']",
        "SELECT [t].[Id], [t].[ProductId]",
        "FROM [ProductContact] t",
        "2022-01-07 14:48:31.572 +01:00 [DBG] Authorization was successful.",
        "2022-01-07 14:48:29.848 +01:00 [DBG] Sending body <?xml version=\"1.0\" encoding=\"utf-16\"?>",
        "<GetProducts xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.product-inventory.xyz/\">",
        "  <ProductId xsi:nil=\"true\" />",
        "</GetProducts> of type LargeFileFilter.Tests.Products"
    };

    public SwitchFilterEvaluatorTests()
    {
        _filterParser = Substitute.For<IFilterItemParser>();
        _testObject = new SwitchFilterEvaluator(_filterParser);
    }

    [Theory]
    [InlineData(_sampleLine1, "The quick", FilterSwitches.StartsWith, true)]
    [InlineData(_sampleLine1, "The quick", FilterSwitches.StartsWith | FilterSwitches.Not, false)]
    [InlineData(_sampleLine1, "brown", FilterSwitches.StartsWith, false)]
    [InlineData(_sampleLine1, "brown", FilterSwitches.Contains, true)]
    [InlineData(_sampleLine1, "brown", FilterSwitches.Contains | FilterSwitches.Not, false)]
    [InlineData(_sampleLine1, "xyz", FilterSwitches.Contains, false)]
    [InlineData(_sampleLine1, "lazy dog.", FilterSwitches.EndsWith, true)]
    [InlineData(_sampleLine1, "lazy dog.", FilterSwitches.EndsWith | FilterSwitches.Not, false)]
    [InlineData(_sampleLine1, "lazy dog", FilterSwitches.EndsWith, false)]
    [InlineData(_sampleLine1, "fox.*over", FilterSwitches.Regex, true)]
    [InlineData(_sampleLine1, "fox.*over", FilterSwitches.Regex | FilterSwitches.Not, false)]
    [InlineData(_sampleLine1, "\\(", FilterSwitches.Regex, false)]
    public void Evaluates_Line_Correctly(string line, string filterText, FilterSwitches switches, bool expectedResult)
    {
        _filterParser.ParseFilterItems().Returns(new List<FilterItem>() { new FilterItem(filterText, switches, FilterSegments.StartsWith) });

        _testObject.IncludeLine(line).Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(1, 8)]
    [InlineData(2, 5)]
    [InlineData(3, 4)]
    public void Evaluates_Lines_Correctly(int settingsId, int expectedLines)
    {
        var dict = new Dictionary<int, AppSettings>
        {
            {1, new() { FilterItems = new FilterItemRaw[] { new() { FilterText = "2022-01-07", Switches = "s" } } }},
            {2, new() { FilterItems = new FilterItemRaw[] { new() { FilterText = "2022-01-07", Switches = "xs" } } }},
            {3, new() { FilterItems = new FilterItemRaw[] { new() { FilterText = "connection to database", Switches = "xc" }, new() { FilterText = "<.*>", Switches = "r" } } }},
        };

        var filterParser = new FilterItemParser(dict[settingsId]);
        var testObject = new SwitchFilterEvaluator(filterParser);

        _sampleLogLines.Where(l => testObject.IncludeLine(l)).Count().Should().Be(expectedLines);
    }

}
