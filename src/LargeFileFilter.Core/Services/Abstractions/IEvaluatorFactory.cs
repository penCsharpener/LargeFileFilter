using LargeFileFilter.Core.Models;
using LargeFileFilter.Core.Services.Evaluators;

namespace LargeFileFilter.Core.Services.Abstractions
{
    public interface IEvaluatorFactory
    {
        Evaluator GetFilter<TFilter>(TFilter setting) where TFilter : BaseFilter;
    }
}
